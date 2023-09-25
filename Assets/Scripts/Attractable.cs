using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Attractable : MonoBehaviour
{
	private static List<Attractable> spawnedAttractables = new();
	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	[SerializeField] private bool destroyUponCollision = true;
	[SerializeField] private float collisionSpeedThreshold = 0.2f;
	[SerializeField] private GameObject impactEffectPrefab;
	[SerializeField] private bool canSpawnFragments = false;

	private GameObject spawnedObjectsContainer;
	private AsteroidSpawner asteroidSpawner;
	private bool hasCollided = false;

	[Serializable] public class CollisionEvent : UnityEvent { }
	[SerializeField] private CollisionEvent OnCollision;

	public Rigidbody rb
	{ get; private set; }

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


	private void SpawnCollisionFragments(Collision collision)
	{
		Bounds bounds = GetComponent<Collider>().bounds;
		float totalFragmentableMass = rb.mass;

		while (totalFragmentableMass > 0.0f)
		{
			Vector3 spawnPoint = Vector3.zero;
			spawnPoint.x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
			spawnPoint.y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

			var newFragment = asteroidSpawner.SpawnFragment(spawnPoint);
			newFragment.rb.velocity = rb.velocity;

			totalFragmentableMass -= newFragment.rb.mass;
		}
	}

	private void SpawnCollisionEffects(Collision collision)
	{
		if (impactEffectPrefab != null)
		{
			bool collidedWithMajorBody = collision.gameObject.GetComponent<Attractor>();
			Vector3 spawnPoint = collision.GetContact(0).point;

			var effect = Instantiate(impactEffectPrefab, spawnPoint, Quaternion.identity);
			effect.transform.up = rb.velocity.normalized;
			effect.transform.parent = spawnedObjectsContainer.transform;

			var vfx = effect.GetComponent<VisualEffect>();
			vfx.SetFloat("fragmentMaxVelocity", rb.velocity.magnitude * 1.5f);
			vfx.SetBool("useAltColor", collidedWithMajorBody);

			if (effect.TryGetComponent<FollowObject>(out var follower))
			{
				if (collidedWithMajorBody)
				{
					follower.objectToFollow = collision.gameObject;
					follower.offset = transform.position - collision.gameObject.transform.position;
				}
				else
				{
					follower.enabled = false;
				}
			}
		}
	}

	private void HandleCollision(Collision collision)
	{
		if (rb.velocity.magnitude > collisionSpeedThreshold)
		{
			hasCollided = true;

			if (canSpawnFragments)
			{
				SpawnCollisionFragments(collision);
			}

			SpawnCollisionEffects(collision);
		}
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		spawnedObjectsContainer = GameObject.FindWithTag("SpawnedObjectsContainer");
		asteroidSpawner = GameObject.FindWithTag("AsteroidSpawner").GetComponent<AsteroidSpawner>();
	}

	private void OnEnable()
	{
		spawnedAttractables.Add(this);
	}

	private void OnDisable()
	{
		spawnedAttractables.Remove(this);
	}

	private void OnCollisionEnter(Collision collision)
	{
		HandleCollision(collision);
	}

	private void Update()
	{
		if (hasCollided)
		{
			OnCollision.Invoke();

			if (destroyUponCollision)
			{
				Destroy(gameObject);
			}
		}
	}
}