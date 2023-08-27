using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour
{
	private static List<Attractable> spawnedAttractables = new();

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	private void OnEnable()
	{
		spawnedAttractables.Add(this);
	}

	private void OnDisable()
	{
		spawnedAttractables.Remove(this);
	}
}
