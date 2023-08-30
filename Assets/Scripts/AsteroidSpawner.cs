using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	private const float spawnDistance = 10.0f;

	[SerializeField] private List<GameObject> asteroidPrefabs = new();

	public void SpawnAsteroid()
	{
		Vector3 spawnPos = transform.position;
		spawnPos += (Vector3)Random.insideUnitCircle.normalized * spawnDistance;

		var randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
		var newAsteroid = Instantiate(randomPrefab, spawnPos, Quaternion.identity, transform);
		newAsteroid.transform.localScale = Vector3.one * Random.Range(0.1f, 0.3f);
	}

	private void Start()
	{
		for (int i = 0; i < 5; i++)
		{
			SpawnAsteroid();
		}
	}
}
