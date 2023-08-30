using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour
{
	private static List<Attractable> spawnedAttractables = new();

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	[SerializeField] private float mass;
	[SerializeField] private Vector3 velocity;
	[SerializeField] private GameObject fragmentPrefab;

	public float Mass => mass;

	public void AddVelocity(Vector3 v)
	{
		velocity += v;
	}

	private void SpawnFragments(int numFragments, Vector3 forces)
	{
		for (int i = 0; i < numFragments; i++)
		{
			var newFragment = Instantiate(fragmentPrefab, transform.position, Quaternion.identity).GetComponent<Attractable>();
			newFragment.AddVelocity(forces);
		}
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
		if (fragmentPrefab != null)
		{
			Vector3 direction = (collision.transform.position - transform.position).normalized;
			Vector3 surfaceNormal = collision.GetContact(0).normal;

			Vector3 reflectionVector = Vector3.Reflect(direction, surfaceNormal);
			reflectionVector = reflectionVector.normalized * 0.75f;

			SpawnFragments(100, reflectionVector);
		}

		Destroy(gameObject);
	}
}