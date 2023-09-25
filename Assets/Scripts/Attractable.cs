using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attractable : MonoBehaviour
{
	protected static List<Attractable> spawnedAttractables = new();
	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	[SerializeField] protected float collisionSpeedThreshold = 0.2f;

	protected GameObject spawnedObjectsContainer;
	protected bool hasCollided = false;

	[Serializable] public class CollisionEvent : UnityEvent { }
	[SerializeField] protected CollisionEvent OnCollision;

	public Rigidbody rb { get; private set; }

	public void DefineOrbit(Rigidbody centralBody, float periapsis)
	{
		Vector3 parentChildDirection = centralBody.transform.position - transform.position;
		float distance = parentChildDirection.magnitude;
		parentChildDirection.Normalize();

		float mu = Attractor.G * centralBody.mass; // gravitational parameter of the central body
		float a = (distance + periapsis) / 2; // length of the semi-major axis of the elliptical orbit
		float orbitalVelocity = Mathf.Sqrt(mu * (2 / distance - 1 / a)); // vis-viva equation
		Vector3 orbitalDirection = Quaternion.Euler(0, 0, 90.0f) * parentChildDirection;

		var velocityVector = orbitalDirection * orbitalVelocity;
		rb.velocity = velocityVector;
	}

	protected virtual void HandleCollision(Collision collision)
	{
		if (rb.velocity.magnitude > collisionSpeedThreshold)
		{
			hasCollided = true;
		}
	}

	protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	protected virtual void OnEnable()
	{
		spawnedAttractables.Add(this);
	}

	protected virtual void OnDisable()
	{
		spawnedAttractables.Remove(this);
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		HandleCollision(collision);
	}

	protected virtual void Start()
	{
		spawnedObjectsContainer = GameObject.FindWithTag("SpawnedObjectsContainer");
	}

	protected virtual void Update()
	{
		if (hasCollided)
		{
			OnCollision.Invoke();
		}
	}
}