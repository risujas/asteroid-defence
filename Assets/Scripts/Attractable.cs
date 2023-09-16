using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Attractable : MonoBehaviour
{
	private static List<Attractable> spawnedAttractables = new();

	public static int RecommendedAttractablesLimit = 300;

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	public static int NumAttractables => SpawnedAttractables.Count;

	public static bool IsAboveRecommendedAttractablesLimit => NumAttractables >= RecommendedAttractablesLimit;

	[SerializeField] private float mass;
	[SerializeField, ReadOnly] private Vector3 velocity;

	[SerializeField] private GameObject impactEffectPrefab;

	// Optional components
	private Fragmentable fragmentable;
	private FragmentTrail fragmentTrail;

	private const float ejectionVfxSpeedMultiplier = 0.35f;

	private bool allowVelocityChange = true;
	private bool hasImpacted = false;
	private Vector3 impactPosition;

	public float Mass
	{
		get { return mass; }
		set { mass = value; }
	}

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

	private void Start()
	{
		fragmentable = GetComponent<Fragmentable>();
		fragmentTrail = GetComponentInChildren<FragmentTrail>();
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
		transform.position += velocity * Time.smoothDeltaTime;

		if (hasImpacted)
		{
			float distance = Vector3.Distance(transform.position, impactPosition);
			if (distance >= transform.lossyScale.x)
			{
				Destroy(gameObject);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		Vector3 reflectionVector = Vector3.Reflect(velocity, collision.GetContact(0).normal);

		SpawnCollisionEffects(collision, reflectionVector);

		if (fragmentable != null)
		{
			fragmentable.SpawnCollisionFragments(collision, reflectionVector, this);
		}

		if (fragmentTrail != null)
		{
			fragmentTrail.DetachTrailFromParent();
		}

		allowVelocityChange = false;
		hasImpacted = true;
		impactPosition = transform.position;

		GetComponent<SphereCollider>().enabled = false;

		DamageNearbyHealthObjects();
	}
}