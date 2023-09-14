using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Attractable : MonoBehaviour
{
	private static List<Attractable> spawnedAttractables = new();

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	[Header("Physical Properties")]
	[SerializeField] private float density;
	[SerializeField] private float mass;
	[SerializeField, ReadOnly] private Vector3 velocity;

	[Header("Prefabs")]
	[SerializeField] private Attractable fragmentPrefab;
	[SerializeField] private GameObject impactEffectPrefab;

	private const float minFragmentSpeedMultiplier = 0.25f;
	private const float maxFragmentSpeedMultiplier = 0.75f;
	private const float ejectionVfxSpeedMultiplier = 0.35f;

	private bool allowVelocityChange = true;
	private bool hasImpacted = false;
	private Vector3 impactPosition;

	public float Mass => mass;

	public Vector3 Velocity => velocity;

	public void AddVelocity(Vector3 v)
	{
		if (allowVelocityChange)
		{
			velocity += v;
		}
	}

	public void SetMassFromDensityAndScale()
	{
		float r = transform.localScale.x / 2.0f;
		float volume = (4.0f / 3.0f) * Mathf.PI * Mathf.Pow(r, 3);
		mass = volume * density;
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
		// TODO
	}

	private void Start()
	{
		SetMassFromDensityAndScale();
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