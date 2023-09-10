using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	[SerializeField] private float spawnHeight = 0.5f;
	[SerializeField] private List<Building> spawnableBuildings = new();
	[SerializeField] private float buildPoints = 100.0f;

	private bool isPlacingBuilding = false;
	private Building placedBuildingPrefab = null;

	public float SpawnHeight => spawnHeight;

	private void Start()
	{
	}

	private void Update()
	{
		if (placedBuildingPrefab == null)
		{
			placedBuildingPrefab = spawnableBuildings[0];
		}

		// NONE OF THIS WORKS

		if (Input.GetMouseButtonDown(0))
		{
			var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mouseWorldPos.z = 0.0f;

			Vector3 spawnDir = (mouseWorldPos - transform.position).normalized;
			Vector3 spawnPos = spawnDir * spawnHeight;

			var newBuilding = Instantiate(placedBuildingPrefab, spawnPos, Quaternion.identity);
			newBuilding.transform.parent = transform;
		}
	}
}
