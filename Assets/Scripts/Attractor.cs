using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
	public const float G = 0.000001f;

	private static List<Attractor> attractors = new();

	public static IReadOnlyList<Attractor> Attractors => attractors.AsReadOnly();

	[SerializeField] private float mass;

	public float Mass => mass;

	private void OnEnable()
	{
		attractors.Add(this);
	}

	private void OnDisable()
	{
		attractors.Remove(this);
	}

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
