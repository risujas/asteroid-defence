using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField] private Attractor centralBody;
	[SerializeField] private float spawnDistance = 10.0f;
	[SerializeField] private List<GameObject> asteroidPrefabs = new List<GameObject>();
	[SerializeField] private float minScale = 0.1f;
	[SerializeField] private float maxScale = 0.2f;

	public void SpawnAsteroid()
	{
		Vector3 spawnPos = transform.position;
		spawnPos += (Vector3)Random.insideUnitCircle.normalized * spawnDistance;

		var randomPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
		var newAsteroid = Instantiate(randomPrefab, spawnPos, Quaternion.identity, transform);
		newAsteroid.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);

		var attractable = newAsteroid.GetComponent<Attractable>();
		DefineAsteroidOrbit(attractable, Random.Range(0.5f, 1.0f), Random.Range(-90.0f, 90.0f));
	}

	private void DefineAsteroidOrbit(Attractable attractable, float velocityModifier, float facing)
	{
		Vector3 vectorAB = centralBody.transform.position - attractable.transform.position;
		float distance = vectorAB.magnitude;
		vectorAB.Normalize();
		Vector3 angledVector = Quaternion.Euler(0, 0, facing) * vectorAB;

		float orbitalVelocity = Mathf.Sqrt(Attractor.G * centralBody.Mass / distance) * velocityModifier;
		var velocityVector = angledVector * orbitalVelocity;

		attractable.AddVelocity(velocityVector);
	}

	private void Start()
	{
		for (int i = 0; i < 100; i++)
		{
			SpawnAsteroid();
		}
	}
}
