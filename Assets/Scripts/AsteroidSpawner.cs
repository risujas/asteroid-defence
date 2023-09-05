using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField] private Attractor centralBody;
	[SerializeField] private float spawnDistance = 100.0f;
	[SerializeField] private float spawnRadius = 20.0f;
	[SerializeField] private float cullDistance = 200.0f;

	[SerializeField] private List<GameObject> asteroidPrefabs = new List<GameObject>();
	[SerializeField] private GameObject fragmentPrefab;

	private Vector3 swarmSpawnPoint;

	private void SpawnSwarm(int numAsteroids, int numFragments, float velocity, float minScale, float maxScale)
	{
		swarmSpawnPoint = transform.position + (Vector3)Random.insideUnitCircle * spawnDistance;
		Vector3 headingVector = GetHeadingVector(swarmSpawnPoint, centralBody.transform.position, 0.0f);

		for (int i = 0; i < numAsteroids; i++)
		{
			var asteroid = SpawnAsteroid(minScale, maxScale);
			DefineTrajectory(asteroid, velocity, headingVector);
		}

		for (int i = 0; i < numFragments; i++)
		{
			var fragment = SpawnFragment();
			DefineTrajectory(fragment, velocity, headingVector);
		}
	}

	private Attractable SpawnAsteroid(float minScale, float maxScale)
	{
		Vector3 spawnPos = swarmSpawnPoint;
		spawnPos += Random.insideUnitSphere * spawnRadius;
		spawnPos.z = 0.0f;

		var randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
		var newAsteroid = Instantiate(randomPrefab, spawnPos, Quaternion.identity, transform);
		newAsteroid.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);

		return newAsteroid.GetComponent<Attractable>();
	}

	private Attractable SpawnFragment()
	{
		Vector3 spawnPos = swarmSpawnPoint;
		spawnPos += Random.insideUnitSphere * spawnRadius;
		spawnPos.z = 0.0f;

		var newAsteroid = Instantiate(fragmentPrefab, spawnPos, Quaternion.identity, transform);

		return newAsteroid.GetComponent<Attractable>();
	}

	private void DefineTrajectory(Attractable attractable, float velocityModifier, Vector3 headingVector)
	{
		Vector3 vectorAB = centralBody.transform.position - attractable.transform.position;
		float distance = vectorAB.magnitude;
		vectorAB.Normalize();

		float orbitalVelocity = Mathf.Sqrt(Attractor.G * centralBody.Mass / distance) * velocityModifier;
		var velocityVector = headingVector * orbitalVelocity;

		attractable.AddVelocity(velocityVector);
	}

	private Vector3 GetHeadingVector(Vector3 fromVector, Vector3 toVector, float headingOffset)
	{
		Vector3 vectorAB = (toVector - fromVector).normalized;
		return Quaternion.Euler(0, 0, headingOffset) * vectorAB;
	}

	private void Start()
	{
		SpawnSwarm(10, 70, 4.0f, 0.1f, 0.2f);
	}

	private void Update()
	{
		for (int i = Attractable.SpawnedAttractables.Count - 1; i >= 0; i--)
		{
			var a = Attractable.SpawnedAttractables[i];
			float distance = Vector3.Distance(transform.position, a.transform.position);

			if (distance >= cullDistance)
			{
				Destroy(a.gameObject);
			}
		}
	}
}
