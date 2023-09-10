using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	[SerializeField] private float spawnHeight = 0.5f;
	[SerializeField] private List<Building> spawnableBuildings = new();


	public float SpawnHeight => spawnHeight;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
