using UnityEngine;

public class Attractor : MonoBehaviour
{
	private const float G = 0.001f;

	[SerializeField] private float mass;

	private void Attract(Attractable a)
	{
		Vector3 direction = transform.position - a.transform.position;
		float distance = direction.magnitude;
		direction.Normalize();

		float force = G * (mass * a.Mass) / (distance * distance);
		float resultingForce = force / a.Mass;
		Vector3 forceVector = direction * resultingForce;

		a.AddVelocity(forceVector * Time.deltaTime);
	}

	private void LateUpdate()
	{
		foreach (var a in Attractable.SpawnedAttractables)
		{
			Attract(a);
		}
	}
}
