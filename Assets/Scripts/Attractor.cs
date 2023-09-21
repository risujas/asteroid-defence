using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
	public const float G = 0.0000004f;

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

		Rigidbody rb = a.GetComponent<Rigidbody>();

		float force = G * (mass * rb.mass) / (distance * distance);
		Vector3 forceVector = direction * force;

		rb.AddForce(forceVector, ForceMode.Force);
	}

	private void FixedUpdate()
	{
		foreach (var a in Attractable.SpawnedAttractables)
		{
			Attract(a);
		}
	}
}
