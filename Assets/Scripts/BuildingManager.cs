using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	[SerializeField] private float spawnHeight = 0.5f;
	[SerializeField] private float buildPoints = 100.0f;

	[SerializeField] private Building FactoryPrefab;
	[SerializeField] private Building CommandCenterPrefab;

	[SerializeField] private Material placementAidMaterialValid;
	[SerializeField] private Material placementAidMaterialInvalid;

	private bool isPlacingBuilding = false;
	private Building selectedBuildingPrefab = null;
	private GameObject placementAidMarker = null;
	private Renderer placementAidMarkerRenderer = null;

	public float SpawnHeight => spawnHeight;

	public void PlaceFactory()
	{
		StartBuildingPlacement(FactoryPrefab);
	}

	public void PlaceCommandCenter()
	{
		StartBuildingPlacement(CommandCenterPrefab);
	}

	private void StartBuildingPlacement(Building building)
	{
		selectedBuildingPrefab = building;
		isPlacingBuilding = true;

		if (placementAidMarker != null)
		{
			Destroy(placementAidMarker);
		}

		placementAidMarker = Instantiate(selectedBuildingPrefab, Vector3.zero, selectedBuildingPrefab.transform.rotation).gameObject;
		Destroy(placementAidMarker.GetComponent<Building>());
		placementAidMarkerRenderer = placementAidMarker.GetComponent<Renderer>();
		placementAidMarkerRenderer.material = placementAidMaterialInvalid;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (isPlacingBuilding)
		{
			var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mouseWorldPos.z = 0.0f;

			placementAidMarker.transform.position = mouseWorldPos;

			//Vector3 spawnDir = (mouseWorldPos - transform.position).normalized;
			//Vector3 spawnPos = transform.position + (spawnDir * spawnHeight);

			//var newBuilding = Instantiate(selectedBuildingPrefab, spawnPos, Quaternion.identity, transform);
		}
	}
}
