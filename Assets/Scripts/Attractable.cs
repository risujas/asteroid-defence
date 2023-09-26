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
	[SerializeField] protected CollisionEvent OnAttractorCollision;
	[SerializeField] protected CollisionEvent OnMinorCollision;
	[SerializeField] protected CollisionEvent OnAnyCollision;

	public Rigidbody rb { get; private set; }

	public void DefineOrbit(Rigidbody centralBody, float periapsis)
	{
		Vector3 parentChildDirection = centralBody.transform.position - transform.position;
		float distance = parentChildDirection.magnitude;
		parentChildDirection.Normalize();

		float orbitalVelocity = OrbitalVelocity.GetOrbitalVelocity(distance, periapsis, centralBody.mass);
		Vector3 orbitalDirection = Quaternion.Euler(0, 0, 90.0f) * parentChildDirection;

		var velocityVector = orbitalDirection * orbitalVelocity;
		rb.velocity = velocityVector;
	}

	protected virtual void HandleCollision(Collision collision)
	{
		if (rb.velocity.magnitude > collisionSpeedThreshold)
		{
			if (collision.gameObject.GetComponent<Attractor>())
			{
				OnAttractorCollision.Invoke();
			}
			else
			{
				OnMinorCollision.Invoke();
			}
			OnAnyCollision.Invoke();
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
}