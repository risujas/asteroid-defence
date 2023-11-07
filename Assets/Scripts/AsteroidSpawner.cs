using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField] private GravityBody centralBody;

	[SerializeField] private bool enableAutoSpawner = false;

	[SerializeField] private List<GravityBody> asteroidPrefabs = new List<GravityBody>();
	[SerializeField] private List<GravityBody> fragmentPrefabs = new List<GravityBody>();

	private float groupSpawnDistance = 30.0f;
	private float groupSpawnRadius = 10.0f;
	private int minGroupSize = 20;
	private int maxGroupSize = 60;

	private float minStartVelocity = 0.1f;
	private float maxStartVelocity = 0.5f;

	private float minScaleMultiplier = 0.75f;
	private float maxScaleMultiplier = 1.1f;

	private GameObject spawnedObjectsContainer;

	public GravityBody SpawnFragment(Vector3 spawnPoint)
	{
		var randomPrefab = fragmentPrefabs[Random.Range(0, fragmentPrefabs.Count)];
		return SpawnAsteroid(spawnPoint, randomPrefab);
	}

	public GravityBody SpawnAsteroid(Vector3 spawnPoint)
	{
		var randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
		return SpawnAsteroid(spawnPoint, randomPrefab);
	}

	private GravityBody SpawnAsteroid(Vector3 spawnPoint, GravityBody prefab)
	{
		float scaleMultiplier = Random.Range(minScaleMultiplier, maxScaleMultiplier);

		var newAsteroid = Instantiate(prefab, spawnPoint, Quaternion.identity, spawnedObjectsContainer.transform);
		newAsteroid.transform.localScale *= scaleMultiplier;
		newAsteroid.rb.mass *= Mathf.Pow(scaleMultiplier, 3);

		return newAsteroid;
	}

	private void SpawnAsteroidGroup(Vector3 groupPos)
	{
		Vector3 groupDir = (centralBody.transform.position - groupPos).normalized;
		float v = Random.Range(minStartVelocity, maxStartVelocity);

		int numToSpawn = Random.Range(minGroupSize, maxGroupSize);
		for (int i = 0; i < numToSpawn; i++)
		{
			Vector3 asteroidPos = groupPos + Random.insideUnitSphere * groupSpawnRadius;
			asteroidPos.z = 0.0f;

			Asteroid asteroid;
			if (Random.value < 0.667f)
			{
				asteroid = SpawnFragment(asteroidPos).GetComponent<Asteroid>();
			}
			else
			{
				asteroid = SpawnAsteroid(asteroidPos).GetComponent<Asteroid>();
			}

			asteroid.rb.velocity = groupDir * v;
		}
	}

	private void CullDistantAsteroids()
	{
		for (int i = GravityBody.GravityBodies.Count - 1; i >= 0; i--)
		{
			var a = GravityBody.GravityBodies[i];
			float distance = Vector3.Distance(transform.position, a.transform.position);

			if (distance >= groupSpawnDistance + groupSpawnRadius)
			{
				Destroy(a.gameObject);
			}
		}
	}

	private void HandleAutoSpawning()
	{
		if (!enableAutoSpawner)
		{
			return;
		}

		Vector3 spawnPoint = centralBody.transform.position + (Quaternion.Euler(0.0f, 0.0f, Random.Range(-180.0f, 180.0f)) * (Vector3.up * groupSpawnDistance));
	}

	private void Awake()
	{
		spawnedObjectsContainer = GameObject.FindWithTag("SpawnedObjectsContainer");
	}

	private void Update()
	{
		CullDistantAsteroids();
		HandleAutoSpawning();
	}
}
