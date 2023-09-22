using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
	public const float G = 0.0000004f;

	private static List<Attractor> attractors = new();

	public static IReadOnlyList<Attractor> Attractors => attractors.AsReadOnly();

	public Rigidbody rb { get; set; }

	private void Attract(Attractable a)
	{
		Vector3 direction = transform.position - a.transform.position;
		float distance = direction.magnitude;
		direction.Normalize();

		float force = G * (rb.mass * a.rb.mass) / (distance * distance);
		Vector3 forceVector = direction * force;

		a.rb.AddForce(forceVector, ForceMode.Force);
	}

	private void OnEnable()
	{
		attractors.Add(this);
	}

	private void OnDisable()
	{
		attractors.Remove(this);
	}

	private void Awake()
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
