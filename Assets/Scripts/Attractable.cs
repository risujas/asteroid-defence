using System.Collections.Generic;
using UnityEngine;
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


	private void SpawnCollisionFragments(Collision collision, Vector3 postCollisionVector)
	{
		float fragmentSpawnRadius = GetComponent<Collider>().bounds.size.x / 2.0f;
		float totalFragmentableMass = rb.mass * 0.5f;

		while (totalFragmentableMass > 0.0f)
		{
			Vector3 spawnPoint = collision.GetContact(0).point + Random.insideUnitSphere.normalized * Random.Range(-fragmentSpawnRadius, fragmentSpawnRadius);
			spawnPoint.z = 0.0f;

			var newFragment = asteroidSpawner.SpawnFragment(spawnPoint);
			newFragment.rb.velocity = Quaternion.AngleAxis(Random.Range(-20.0f, 20.0f), Vector3.forward) * postCollisionVector;

			totalFragmentableMass -= newFragment.rb.mass;
		}
	}

	private void SpawnCollisionEffects(Collision collision, Vector3 postCollisionVector)
	{
		if (impactEffectPrefab != null)
		{
			bool collidedWithMajorBody = collision.gameObject.GetComponent<Attractor>();
			Vector3 spawnPoint = collision.GetContact(0).point;

			var effect = Instantiate(impactEffectPrefab, spawnPoint, Quaternion.identity);
			effect.transform.up = postCollisionVector;
			effect.transform.parent = spawnedObjectsContainer.transform;

			var vfx = effect.GetComponent<VisualEffect>();
			vfx.SetFloat("fragmentMaxVelocity", postCollisionVector.magnitude);
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
		float relV = rb.velocity.magnitude;
		if (collision.rigidbody != null)
		{
			relV = (collision.rigidbody.velocity - rb.velocity).magnitude;
		}

		if (relV > collisionSpeedThreshold)
		{
			Vector3 postCollisionVector;

			if (collision.gameObject.GetComponent<Attractor>() != null)
			{
				postCollisionVector = Vector3.Reflect(rb.velocity, collision.GetContact(0).normal);
			}
			else
			{
				// TODO this is physically inaccurate and cancels out velocity for no reason if the other body's velocity is different
				// need to properly calculate the velocity while taking into account the masses and forces involved

				float totalMass = rb.mass + collision.rigidbody.mass;

				Vector3 compoundVelocity = Vector3.zero;
				compoundVelocity += rb.velocity * (rb.mass / totalMass);
				compoundVelocity += collision.rigidbody.velocity * (collision.rigidbody.mass / totalMass);

				postCollisionVector = compoundVelocity;
			}

			if (canSpawnFragments)
			{
				SpawnCollisionFragments(collision, postCollisionVector);
			}

			SpawnCollisionEffects(collision, postCollisionVector * 1.5f);

			if (destroyUponCollision)
			{
				Destroy(gameObject);
			}
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
}