using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
	public const float G = 0.0000004f;

	private static List<Attractor> attractors = new();

	public static IReadOnlyList<Attractor> Attractors => attractors.AsReadOnly();

	private Rigidbody rb;

	private void Attract(Attractable a)
	{
		Vector3 direction = transform.position - a.transform.position;
		float distance = direction.magnitude;
		direction.Normalize();

		Rigidbody attractableRb = a.GetComponent<Rigidbody>();

		float force = G * (rb.mass * attractableRb.mass) / (distance * distance);
		Vector3 forceVector = direction * force;

		attractableRb.AddForce(forceVector, ForceMode.Force);
	}

	private void OnEnable()
	{
		attractors.Add(this);
	}

	private void OnDisable()
	{
		attractors.Remove(this);
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		foreach (var a in Attractable.SpawnedAttractables)
		{
			if (a.gameObject == gameObject)
			{
				continue;
			}

			Attract(a);
		}
	}
}
