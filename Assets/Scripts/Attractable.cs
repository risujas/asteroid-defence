using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Attractable : MonoBehaviour
{
	private static List<Attractable> spawnedAttractables = new();

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	[SerializeField] private float mass;
	[SerializeField, ReadOnly] private Vector3 velocity;

	[SerializeField] private Attractable fragmentPrefab;
	[SerializeField] private ImpactLight impactLightPrefab;
	[SerializeField] private VisualEffect vfxPrefab;

	public float Mass => mass;

	public Vector3 Velocity => velocity;

	public void AddVelocity(Vector3 v)
	{
		velocity += v;
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
	}

	void OnCollisionEnter(Collision collision)
	{
		if (vfxPrefab != null)
		{
			Vector3 spawnPoint = collision.GetContact(0).point;
			var vfx = Instantiate(vfxPrefab, spawnPoint, Quaternion.identity, collision.gameObject.transform);
			vfx.transform.up = collision.GetContact(0).normal;
		}

		if (impactLightPrefab != null)
		{
			Vector3 spawnPoint = collision.GetContact(0).point + collision.GetContact(0).normal * 0.025f;
			Instantiate(impactLightPrefab, spawnPoint, Quaternion.identity, collision.gameObject.transform);
		}

		if (fragmentPrefab != null)
		{
			Vector3 surfaceNormal = collision.GetContact(0).normal;
			Vector3 reflectionVector = Vector3.Reflect(velocity, surfaceNormal);

			int numFragments = Mathf.RoundToInt(mass / fragmentPrefab.Mass);
			for (int i = 0; i < numFragments; i++)
			{
				Vector3 individualVector = reflectionVector * Random.Range(0.0f, 0.8f);
				individualVector = Quaternion.AngleAxis(Random.Range(-30.0f, 30.0f), Vector3.forward) * individualVector;

				float width = transform.localScale.x * 0.5f;
				Vector3 spawnPoint = collision.GetContact(0).point + collision.GetContact(0).normal * 0.03f;
				Vector3 spawnPosition = spawnPoint += Random.insideUnitSphere * Random.Range(-width, width);
				spawnPosition.z = 0.0f;

				var newFragment = Instantiate(fragmentPrefab, spawnPosition, Quaternion.identity, transform.parent).GetComponent<Attractable>();
				newFragment.AddVelocity(individualVector);
			}
		}

		Destroy(gameObject);
	}
}