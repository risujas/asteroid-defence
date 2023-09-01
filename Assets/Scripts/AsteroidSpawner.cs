using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField] private float spawnDistance = 10.0f;
	[SerializeField] private List<GameObject> asteroidPrefabs = new();
	[SerializeField] private float minScale = 0.1f;
	[SerializeField] private float maxScale = 0.2f;

	public void SpawnAsteroid()
	{
		Vector3 spawnPos = transform.position;
		spawnPos += (Vector3)Random.insideUnitCircle.normalized * spawnDistance;

		var randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
		var newAsteroid = Instantiate(randomPrefab, spawnPos, Quaternion.identity, transform);
		newAsteroid.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
	}

	private void Start()
	{
		for (int i = 0; i < 5; i++)
		{
			SpawnAsteroid();
		}
	}
}
