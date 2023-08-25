using UnityEngine;

public class Attractor : MonoBehaviour
{
	private const float G = 0.01f;

	private AsteroidManager asteroidManager;
	private Rigidbody rigidBody;

	private void AttractAsteroids()
	{
		foreach (var a in asteroidManager.SpawnedAsteroids)
		{
			Vector3 direction = transform.position - a.transform.position;
			float distance = direction.magnitude;
			direction.Normalize();

			Vector3 force = direction * G * (rigidBody.mass * a.mass) / (distance * distance);
			a.AddForce(force * Time.deltaTime, ForceMode.Acceleration);
		}
	}

	private void Start()
	{
		asteroidManager = FindObjectOfType<AsteroidManager>();
		rigidBody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		AttractAsteroids();
	}
}
