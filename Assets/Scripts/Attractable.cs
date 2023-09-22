using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour
{
	protected static List<Attractable> spawnedAttractables = new();

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	[SerializeField] private bool destroyUponCollision = true;

	protected virtual void HandleCollision()
	{
		if (destroyUponCollision)
		{
			Destroy(gameObject);
		}
	}

	protected void OnEnable()
	{
		spawnedAttractables.Add(this);
	}

	protected void OnDisable()
	{
		spawnedAttractables.Remove(this);
	}

	protected void OnCollisionEnter(Collision collision)
	{
		HandleCollision();
	}
}