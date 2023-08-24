using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
	private const float G = 0.01f;

	[SerializeField] private Rigidbody asteroidAttractor;
	[SerializeField] private List<GameObject> asteroidPrefabs = new();

	private List<Rigidbody> spawnedAsteroids = new();
	private const float spawnDistance = 20.0f;

	private void SpawnAsteroid()
	{
		Vector3 spawnPos = transform.position;
		spawnPos += (Vector3)Random.insideUnitCircle.normalized * spawnDistance;

		var randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
		var newAsteroid = Instantiate(randomPrefab, spawnPos, Quaternion.identity, transform);
		spawnedAsteroids.Add(newAsteroid.GetComponent<Rigidbody>());

		newAsteroid.transform.localScale = Vector3.one * Random.Range(0.05f, 0.25f);
	}

	private void AttractAsteroids()
	{
		foreach (var a in spawnedAsteroids)
		{
			Vector3 direction = asteroidAttractor.transform.position - a.transform.position;
			float distance = direction.magnitude;
			direction.Normalize();

			Vector3 force = direction * G * (asteroidAttractor.mass * a.mass) / (distance * distance);
			a.AddForce(force * Time.deltaTime, ForceMode.Acceleration);
		}
	}

	private void Start()
	{
		for (int i = 0; i < 50; i++)
		{
			SpawnAsteroid();
		}
	}

	private void FixedUpdate()
	{
		AttractAsteroids();
	}
}
