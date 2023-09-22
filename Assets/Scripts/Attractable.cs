using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Attractable : MonoBehaviour
{
	private static List<Attractable> spawnedAttractables = new();

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	[SerializeField] private bool destroyUponCollision = true;
	[SerializeField] private float collisionSpeedThreshold = 0.2f;
	[SerializeField] private Attractable fragmentPrefab;
	[SerializeField] private FragmentTrail fragmentTrail;
	[SerializeField] private GameObject impactEffectPrefab;

	private GameObject spawnedObjectsContainer;

	public Rigidbody rb { get; private set; }

	private void SpawnCollisionFragments(Collision collision, Vector3 postCollisionVector)
	{
		if (fragmentPrefab == null)
		{
			return;
		}

		Rigidbody fragmentRb = fragmentPrefab.GetComponent<Rigidbody>();

		int numFragments = Mathf.RoundToInt(rb.mass / fragmentRb.mass) / 2;
		for (int i = 0; i < numFragments; i++)
		{
			Vector3 ejectionVector = Quaternion.AngleAxis(Random.Range(-30.0f, 30.0f), Vector3.forward) * postCollisionVector;

			float width = transform.localScale.x * 0.5f;
			Vector3 spawnPoint = (collision.GetContact(0).point + collision.GetContact(0).normal * 0.05f) + (Random.insideUnitSphere.normalized * Random.Range(-width, width));
			spawnPoint.z = 0.0f;

			var newFragmentRb = Instantiate(fragmentPrefab, spawnPoint, Quaternion.identity, transform.parent).GetComponent<Rigidbody>();
			newFragmentRb.velocity = ejectionVector;
		}
	}

	private void SpawnCollisionEffects(Collision collision, Vector3 postCollisionVector)
	{
		if (impactEffectPrefab != null)
		{
			Vector3 spawnPoint = collision.GetContact(0).point;

			var effect = Instantiate(impactEffectPrefab, spawnPoint, Quaternion.identity);
			effect.transform.up = postCollisionVector;
			effect.transform.parent = spawnedObjectsContainer.transform;

			var vfx = effect.GetComponent<VisualEffect>();
			if (vfx != null)
			{
				if (vfx.HasFloat("ejectionSpeed"))
				{
					Vector3 velocity = postCollisionVector * 0.35f;
					vfx.SetFloat("ejectionSpeed", velocity.magnitude);
				}
			}

			var follower = effect.GetComponent<FollowObject>();
			if (follower != null)
			{
				if (collision.gameObject.GetComponent<Attractor>())
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
				postCollisionVector = Vector3.Reflect(rb.velocity, collision.GetContact(0).normal) * Random.Range(0.25f, 0.75f);
			}
			else
			{
				float totalMass = rb.mass + collision.rigidbody.mass;

				Vector3 compoundVelocity = Vector3.zero;
				compoundVelocity += rb.velocity * (rb.mass / totalMass);
				compoundVelocity += collision.rigidbody.velocity * (collision.rigidbody.mass / totalMass);

				postCollisionVector = compoundVelocity * Random.Range(0.8f, 1.0f);
			}

			SpawnCollisionFragments(collision, postCollisionVector);
			SpawnCollisionEffects(collision, postCollisionVector);

			if (fragmentTrail != null)
			{
				fragmentTrail.DetachTrailFromParent();
			}

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
		if (fragmentTrail == null)
		{
			fragmentTrail = GetComponentInChildren<FragmentTrail>();
		}

		spawnedObjectsContainer = GameObject.FindWithTag("SpawnedObjectsContainer");
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