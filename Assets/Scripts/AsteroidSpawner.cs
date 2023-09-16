using System;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[Serializable]
	public class AsteroidSwarm
	{
		public int numAsteroids;
		public int numFragments;
		public float velocity;
		public float minAsteroidScale;
		public float maxAsteroidScale;
	}

	[SerializeField] private Attractor centralBody;
	[SerializeField] private float swarmSpawnDistance = 100.0f;
	[SerializeField] private float swarmSpawnRadius = 20.0f;
	[SerializeField] private float cullDistance = 200.0f;

	[SerializeField] private List<GameObject> asteroidPrefabs = new List<GameObject>();
	[SerializeField] private GameObject fragmentPrefab;

	[SerializeField] private List<AsteroidSwarm> asteroidSwarms = new();

	private Vector3 swarmSpawnPoint;

	private void SpawnNextSwarm()
	{
		if (asteroidSwarms.Count < 1)
		{
			return;
		}

		var swarm = asteroidSwarms[0];
		asteroidSwarms.RemoveAt(0);

		swarmSpawnPoint = centralBody.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * swarmSpawnDistance;
		Vector3 headingVector = GetHeadingVector(swarmSpawnPoint, centralBody.transform.position, 0.0f);

		for (int i = 0; i < swarm.numAsteroids; i++)
		{
			var asteroid = SpawnAsteroid(swarm.minAsteroidScale, swarm.maxAsteroidScale);
			DefineTrajectory(asteroid, swarm.velocity, headingVector);
		}

		for (int i = 0; i < swarm.numFragments; i++)
		{
			var fragment = SpawnFragment();
			DefineTrajectory(fragment, swarm.velocity, headingVector);
		}
	}

	private Attractable SpawnAsteroid(float minScale, float maxScale)
	{
		Vector3 spawnPos = swarmSpawnPoint;
		spawnPos += UnityEngine.Random.insideUnitSphere * swarmSpawnRadius;
		spawnPos.z = 0.0f;

		var randomPrefab = asteroidPrefabs[UnityEngine.Random.Range(0, asteroidPrefabs.Count)];
		var newAsteroid = Instantiate(randomPrefab, spawnPos, Quaternion.identity, transform);
		newAsteroid.transform.localScale = Vector3.one * UnityEngine.Random.Range(minScale, maxScale);

		var dynamicMass = newAsteroid.GetComponent<DynamicMass>();
		dynamicMass.SetMassFromDensityAndScale();

		return newAsteroid.GetComponent<Attractable>();
	}

	private Attractable SpawnFragment()
	{
		Vector3 spawnPos = swarmSpawnPoint;
		spawnPos += UnityEngine.Random.insideUnitSphere * swarmSpawnRadius;
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
		SpawnNextSwarm();
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
