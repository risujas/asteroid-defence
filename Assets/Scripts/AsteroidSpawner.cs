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
	[SerializeField] private int attractablesLimit = 100;
	[SerializeField] private List<GravityBody> asteroidPrefabs = new List<GravityBody>();
	[SerializeField] private List<GravityBody> fragmentPrefabs = new List<GravityBody>();

	private IntervalChanceTimer spawnerTimer = new(1.0f, 1.0f);
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

	private void Start()
	{
		spawnedObjectsContainer = GameObject.FindWithTag("SpawnedObjectsContainer");
	}

	private void Update()
	{
		CullDistantAsteroids();

		if (GravityBody.GravityBodies.Count < attractablesLimit && spawnerTimer.Tick())
		{
			float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
			Vector3 spawnPoint = centralBody.transform.position + (Quaternion.Euler(0.0f, 0.0f, Random.Range(-180.0f, 180.0f)) * (Vector3.up * spawnDistance));

			var asteroid = SpawnAsteroid(spawnPoint);
			asteroid.DefineOrbit(centralBody.rb, 2.0f);
		}
	}
}
