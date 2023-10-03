using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField] private GravityBody centralBody;

	[SerializeField] private float cullingDistance = 200.0f;
	[SerializeField] private float minSpawnDistance = 30.0f;
	[SerializeField] private float maxSpawnDistance = 100.0f;

	[SerializeField] private float asteroidScaleMin = 0.8f;
	[SerializeField] private float asteroidScaleMax = 1.1f;
	[SerializeField] private float fragmentScaleMin = 0.75f;
	[SerializeField] private float fragmentScaleMax = 1.0f;

	[SerializeField] private List<GravityBody> asteroidPrefabs = new List<GravityBody>();
	[SerializeField] private List<GravityBody> fragmentPrefabs = new List<GravityBody>();

	[SerializeField] private float groupSpawnRadius = 10.0f;
	[SerializeField] private int minGroupSize = 20;
	[SerializeField] private int maxGroupSize = 60;

	[SerializeField] private float flybyMinVelocity = 0.1f;
	[SerializeField] private float flybyMaxVelocity = 0.5f;

	[SerializeField] private float lonerSpawnChance = 0.01f;

	private List<Asteroid> previouslySpawnedGroup = null;
	private int previousGroupSize = 0;

	private IntervalTimer spawnTimer = new IntervalTimer(1.0f);
	private GameObject spawnedObjectsContainer;

	public GravityBody SpawnFragment(Vector3 spawnPoint)
	{
		var randomPrefab = fragmentPrefabs[Random.Range(0, fragmentPrefabs.Count)];
		return SpawnAsteroid(spawnPoint, fragmentScaleMin, fragmentScaleMax, randomPrefab);
	}

	public GravityBody SpawnAsteroid(Vector3 spawnPoint)
	{
		var randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
		return SpawnAsteroid(spawnPoint, asteroidScaleMin, asteroidScaleMax, randomPrefab);
	}

	private GravityBody SpawnAsteroid(Vector3 spawnPoint, float minScaleMultiplier, float maxScaleMultiplier, GravityBody prefab)
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
		float v = Random.Range(flybyMinVelocity, flybyMaxVelocity);

		int numToSpawn = Random.Range(minGroupSize, maxGroupSize);

		previouslySpawnedGroup = new();
		previousGroupSize = numToSpawn;

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
			previouslySpawnedGroup.Add(asteroid);
		}
	}

	private void CullDistantAsteroids()
	{
		for (int i = GravityBody.GravityBodies.Count - 1; i >= 0; i--)
		{
			var a = GravityBody.GravityBodies[i];
			float distance = Vector3.Distance(transform.position, a.transform.position);

			if (distance >= cullingDistance)
			{
				Destroy(a.gameObject);
			}
		}
	}

	private void UpdateAsteroidGroupStatus()
	{
		if (previouslySpawnedGroup != null)
		{
			for (int i = previouslySpawnedGroup.Count - 1; i >= 0; i--)
			{
				if (previouslySpawnedGroup[i] == null)
				{
					previouslySpawnedGroup.RemoveAt(i);
				}
			}
		}
	}

	private void Start()
	{
		spawnedObjectsContainer = GameObject.FindWithTag("SpawnedObjectsContainer");
	}

	private void Update()
	{
		CullDistantAsteroids();
		UpdateAsteroidGroupStatus();

		if (spawnTimer.Tick())
		{
			float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
			Vector3 spawnPoint = centralBody.transform.position + (Quaternion.Euler(0.0f, 0.0f, Random.Range(-180.0f, 180.0f)) * (Vector3.up * spawnDistance));

			if (previouslySpawnedGroup == null || ((float)previouslySpawnedGroup.Count / previousGroupSize) < 0.8f)
			{
				float groupSpawnChance = 1.0f - Mathf.Clamp01(GravityBody.GravityBodies.Count / maxGroupSize);
				if (Random.value < groupSpawnChance)
				{
					SpawnAsteroidGroup(spawnPoint);
				}
			}

			if (Random.value < lonerSpawnChance)
			{
				var asteroid = SpawnAsteroid(spawnPoint);
				asteroid.DefineFlyby(centralBody.rb, 10.0f, Random.Range(flybyMinVelocity, flybyMaxVelocity));
			}
		}
	}
}
