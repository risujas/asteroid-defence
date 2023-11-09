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

	private Vector3 currentGroupPos = Vector3.zero;
	private Vector3 currentGroupDir = Vector3.zero;
	private int currentGroupMemberCount = 0;
	private int currentGroupMemberMax = 0;
	private float currentGroupVelocity = 0.0f;

	private float minStartVelocity = 0.1f;
	private float maxStartVelocity = 0.5f;

	private float minScaleMultiplier = 0.75f;
	private float maxScaleMultiplier = 1.1f;

	private IntervalTimer spawnTimer = new IntervalTimer(1.0f);

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

		if (spawnTimer.Tick())
		{
			if (currentGroupMemberCount >= currentGroupMemberMax || currentGroupMemberMax == 0)
			{
				currentGroupMemberCount = 0;
				currentGroupMemberMax = Random.Range(minGroupSize, maxGroupSize);

				currentGroupPos = centralBody.transform.position + (Quaternion.Euler(0.0f, 0.0f, Random.Range(-180.0f, 180.0f)) * (Vector3.up * groupSpawnDistance));
				currentGroupDir = (centralBody.transform.position - currentGroupPos).normalized;
				currentGroupVelocity = Random.Range(minStartVelocity, maxStartVelocity);
			}

			// spawn asteroids until group is filled
		}
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
