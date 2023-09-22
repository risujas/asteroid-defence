using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Attractable : MonoBehaviour
{
	private static List<Attractable> spawnedAttractables = new();

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	private const float minFragmentSpeedMultiplier = 0.5f;
	private const float maxFragmentSpeedMultiplier = 1.0f;
	private const float ejectionVfxSpeedMultiplier = 0.35f;

	[SerializeField] private bool destroyUponCollision = true;
	[SerializeField] private float collisionSpeedThreshold = 0.2f;
	[SerializeField] private Attractable fragmentPrefab;
	[SerializeField] private FragmentTrail fragmentTrail;
	[SerializeField] private GameObject impactEffectPrefab;

	private GameObject spawnedObjectsContainer;
	private Rigidbody rb;

	private void SpawnCollisionFragments(Collision collision, Vector3 reflectionVector)
	{
		if (fragmentPrefab == null)
		{
			return;
		}

		Rigidbody fragmentPfRb = fragmentPrefab.GetComponent<Rigidbody>();
		int numFragments = Mathf.RoundToInt(rb.mass / fragmentPfRb.mass) / 2;

		for (int i = 0; i < numFragments; i++)
		{
			if (rb.mass <= fragmentPfRb.mass)
			{
				break;
			}

			Vector3 individualVector = reflectionVector * Random.Range(minFragmentSpeedMultiplier, maxFragmentSpeedMultiplier);
			individualVector = Quaternion.AngleAxis(Random.Range(-30.0f, 30.0f), Vector3.forward) * individualVector;
			individualVector += collision.rigidbody.velocity;

			float width = transform.localScale.x * 0.5f;
			Vector3 spawnPoint = (collision.GetContact(0).point + collision.GetContact(0).normal * 0.05f) + (Random.insideUnitSphere.normalized * Random.Range(-width, width));
			spawnPoint.z = 0.0f;

			var newFragmentRb = Instantiate(fragmentPrefab, spawnPoint, Quaternion.identity, transform.parent).GetComponent<Rigidbody>();
			newFragmentRb.velocity = individualVector;
		}
	}

	private void SpawnCollisionEffects(Collision collision, Vector3 reflectionVector)
	{
		if (impactEffectPrefab != null)
		{
			Vector3 spawnPoint = collision.GetContact(0).point;

			var effect = Instantiate(impactEffectPrefab, spawnPoint, Quaternion.identity);
			effect.transform.up = reflectionVector;
			effect.transform.parent = spawnedObjectsContainer.transform;

			var vfx = effect.GetComponent<VisualEffect>();
			if (vfx != null)
			{
				if (vfx.HasFloat("ejectionSpeed"))
				{
					Vector3 velocity = (reflectionVector + collision.rigidbody.velocity).normalized * ejectionVfxSpeedMultiplier;
					vfx.SetFloat("ejectionSpeed", velocity.magnitude);
				}
			}

			var follower = effect.GetComponent<FollowObject>();
			if (follower != null)
			{
				// We can only follow an Attractor game object
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
			Vector3 reflectionVector = Vector3.Reflect(rb.velocity, collision.GetContact(0).normal);

			SpawnCollisionFragments(collision, reflectionVector);
			SpawnCollisionEffects(collision, reflectionVector);

			ImpactDamage.DamageNearbyHealthObjects(rb);

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

	private void Start()
	{
		rb = GetComponent<Rigidbody>();

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