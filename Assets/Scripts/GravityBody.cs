using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GravityBody : MonoBehaviour
{
	public const float G = 0.000000000066743f;

	protected static List<GravityBody> gravityBodies = new();
	public static IReadOnlyList<GravityBody> GravityBodies => gravityBodies.AsReadOnly();

	[SerializeField] protected bool useCollisionSpeedThreshold = false;
	[SerializeField] protected float collisionSpeedThreshold = 0.2f;
	[SerializeField] protected bool isMajorBody = false;
	[SerializeField] protected bool isImmuneToMassReduction = false;

	[Serializable] public class CollisionEvent : UnityEvent<Collision> { }
	[SerializeField] protected CollisionEvent OnMajorCollision;
	[SerializeField] protected CollisionEvent OnMinorCollision;
	[SerializeField] protected CollisionEvent OnAnyCollision;

	protected GameObject spawnedObjectsContainer;
	protected bool hasCollided = false;

	public bool IsImmuneToMassReduction => isImmuneToMassReduction;
	public bool UseCollisionSpeedThreshold { get { return useCollisionSpeedThreshold; } set { useCollisionSpeedThreshold = value; } }
	public float CollisionSpeedThreshold { get { return collisionSpeedThreshold; } set { collisionSpeedThreshold = value; } }
	public Rigidbody rb { get; private set; }
	public bool IsMajorBody => isMajorBody;

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

	public void DefineFlyby(Rigidbody centralBody, float radiusAroundBody, float velocity)
	{
		Vector3 targetPoint = centralBody.transform.position + (Vector3)(UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(0.0f, radiusAroundBody));
		Vector3 targetDirection = (targetPoint - transform.position).normalized;
		rb.velocity = targetDirection * velocity;
	}

	public void SetRandomAngularVelocity(float min, float max)
	{
		rb.angularVelocity = new Vector3(0.0f, 0.0f, Mathf.Deg2Rad * UnityEngine.Random.Range(min, max));
	}

	public void Attract(GravityBody a)
	{
		Vector3 direction = transform.position - a.transform.position;
		float distance = direction.magnitude;
		direction.Normalize();

		float force = G * (rb.mass * a.rb.mass) / (distance * distance);
		Vector3 forceVector = direction * force;

		a.rb.AddForce(forceVector, ForceMode.Force);
	}

	protected virtual void HandleCollision(Collision collision)
	{
		if (rb.velocity.magnitude >= collisionSpeedThreshold || !useCollisionSpeedThreshold)
		{
			if (collision.gameObject.GetComponent<GravityBody>().IsMajorBody)
			{
				OnMajorCollision.Invoke(collision);
			}
			else
			{
				OnMinorCollision.Invoke(collision);
			}
			OnAnyCollision.Invoke(collision);
		}
	}

	protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	protected virtual void OnEnable()
	{
		gravityBodies.Add(this);
	}

	protected virtual void OnDisable()
	{
		gravityBodies.Remove(this);
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		HandleCollision(collision);
	}

	protected virtual void Start()
	{
		spawnedObjectsContainer = GameObject.FindWithTag("SpawnedObjectsContainer");
	}

	protected void FixedUpdate()
	{
		if (IsMajorBody)
		{
			foreach (var b in GravityBodies)
			{
				if (b.gameObject == gameObject)
				{
					continue;
				}

				Attract(b);
			}
		}
	}
}