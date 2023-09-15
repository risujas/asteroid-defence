using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Attractable : MonoBehaviour
{
	protected static List<Attractable> spawnedAttractables = new();

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	[Header("Physical Properties")]
	[SerializeField] protected float mass;
	[SerializeField, ReadOnly] protected Vector3 velocity;

	[Header("Prefabs")]
	[SerializeField] protected Attractable fragmentPrefab;
	[SerializeField] protected GameObject impactEffectPrefab;

	protected const float minFragmentSpeedMultiplier = 0.25f;
	protected const float maxFragmentSpeedMultiplier = 0.75f;
	protected const float ejectionVfxSpeedMultiplier = 0.35f;

	protected bool allowVelocityChange = true;
	protected bool hasImpacted = false;
	protected Vector3 impactPosition;

	public float Mass => mass;

	public Vector3 Velocity => velocity;

	public void AddVelocity(Vector3 v)
	{
		if (allowVelocityChange)
		{
			velocity += v;
		}
	}

	private void SpawnCollisionEffects(Collision collision, Vector3 reflectionVector)
	{
		if (impactEffectPrefab != null)
		{
			Vector3 spawnPoint = collision.GetContact(0).point;

			var effect = Instantiate(impactEffectPrefab, spawnPoint, Quaternion.identity);
			effect.transform.up = reflectionVector;

			var vfx = effect.GetComponent<VisualEffect>();
			if (vfx != null)
			{
				if (vfx.HasFloat("ejectionSpeed"))
				{
					vfx.SetFloat("ejectionSpeed", reflectionVector.magnitude * ejectionVfxSpeedMultiplier);
				}
			}

			var follower = effect.GetComponent<FollowObject>();
			if (follower != null)
			{
				follower.objectToFollow = collision.gameObject;
				follower.offset = transform.position - collision.gameObject.transform.position;
			}
		}
	}

	private void SpawnCollisionFragments(Collision collision, Vector3 reflectionVector)
	{
		if (fragmentPrefab != null)
		{
			int numFragments = Mathf.RoundToInt(mass / fragmentPrefab.Mass);
			if (numFragments > 50)
			{
				numFragments = 50;
			}

			for (int i = 0; i < numFragments; i++)
			{
				Vector3 individualVector = reflectionVector * Random.Range(minFragmentSpeedMultiplier, maxFragmentSpeedMultiplier);
				individualVector = Quaternion.AngleAxis(Random.Range(-30.0f, 30.0f), Vector3.forward) * individualVector;

				float width = transform.localScale.x * 0.5f;
				Vector3 spawnPoint = (collision.GetContact(0).point + collision.GetContact(0).normal * 0.05f) + (Random.insideUnitSphere.normalized * Random.Range(-width, width));
				spawnPoint.z = 0.0f;

				var newFragment = Instantiate(fragmentPrefab, spawnPoint, Quaternion.identity, transform.parent).GetComponent<Attractable>();
				newFragment.AddVelocity(individualVector);
			}
		}
	}

	private void DamageNearbyHealthObjects()
	{
		float radius = transform.lossyScale.x * 4.0f;
		float damage = mass * velocity.magnitude;

		foreach (var h in Health.HealthObjects)
		{
			float distance = Vector3.Distance(transform.position, h.transform.position);
			if (distance <= radius)
			{
				float effectiveDamage = damage / (distance * distance);
				h.ChangeHealth(-effectiveDamage);
			}
		}
	}

	protected virtual void Start()
	{
	}

	private void OnEnable()
	{
		spawnedAttractables.Add(this);
	}

	private void OnDisable()
	{
		spawnedAttractables.Remove(this);
	}

	private void Update()
	{
		transform.position += velocity * Time.deltaTime;

		if (hasImpacted)
		{
			float distance = Vector3.Distance(transform.position, impactPosition);
			if (distance >= transform.lossyScale.x)
			{
				Destroy(gameObject);
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		Vector3 reflectionVector = Vector3.Reflect(velocity, collision.GetContact(0).normal);

		SpawnCollisionEffects(collision, reflectionVector);
		SpawnCollisionFragments(collision, reflectionVector);

		allowVelocityChange = false;
		hasImpacted = true;
		impactPosition = transform.position;

		GetComponent<SphereCollider>().enabled = false;

		DamageNearbyHealthObjects();
	}
}