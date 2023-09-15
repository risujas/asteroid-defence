using UnityEngine;
using UnityEngine.UI;

public class BuildingPlacer : MonoBehaviour
{
	[SerializeField] private float buildPoints = 100.0f;

	[Header("Button References")]
	[SerializeField] private Button factoryButton;
	[SerializeField] private Button commandCenterButton;

	[Header("Building Prefabs")]
	[SerializeField] private Building factoryPrefab;
	[SerializeField] private Building commandCenterPrefab;

	[Header("Placement Aid")]
	[SerializeField] private Material placementAidMaterialValid;
	[SerializeField] private Material placementAidMaterialInvalid;
	[SerializeField] private LayerMask buildingSnappingLayerMask;
	[SerializeField] private TimescaleChanger timescaleChangerReference;

	private bool isPlacingBuilding = false;
	private Building selectedBuildingPrefab = null;
	private GameObject placementAidMarker = null;
	private Renderer placementAidMarkerRenderer = null;

	public void PlaceFactory()
	{
		StartBuildingPlacement(factoryPrefab);
	}

	public void PlaceCommandCenter()
	{
		StartBuildingPlacement(commandCenterPrefab);
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
		placementAidMarker.transform.parent = transform;
		Destroy(placementAidMarker.GetComponent<Building>());
		placementAidMarkerRenderer = placementAidMarker.GetComponent<Renderer>();
	}

	private void FinalizeBuildingPlacement(BuildingAnchor anchor)
	{
		var newBuilding = Instantiate(selectedBuildingPrefab, placementAidMarker.transform.position, placementAidMarker.transform.rotation);
		newBuilding.transform.parent = anchor.transform;

		buildPoints -= selectedBuildingPrefab.BuildCost;
	}

	private void CancelBuildingPlacement()
	{
		isPlacingBuilding = false;
		selectedBuildingPrefab = null;

		Destroy(placementAidMarker);
	}

	private void EnableButtons()
	{
		factoryButton.interactable = buildPoints >= factoryPrefab.BuildCost;
		commandCenterButton.interactable = buildPoints >= commandCenterPrefab.BuildCost;
	}

	private void HandlePlacement()
	{
		if (isPlacingBuilding)
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			var colliders = Physics.OverlapSphere(mousePos, 0.1f, buildingSnappingLayerMask);
			if (colliders == null || colliders.Length == 0)
			{
				placementAidMarker.transform.position = mousePos;
				placementAidMarker.transform.up = Vector3.up;
				placementAidMarkerRenderer.material = placementAidMaterialInvalid;
				timescaleChangerReference.SetTimescale(timescaleChangerReference.Level);
			}
			else
			{
				var anchor = colliders[0].GetComponent<BuildingAnchor>();
				Vector2 dir = (mousePos - (Vector2)anchor.transform.position).normalized;

				placementAidMarker.transform.position = dir * anchor.SpawnHeight;
				placementAidMarker.transform.up = dir;
				placementAidMarkerRenderer.material = placementAidMaterialValid;
				timescaleChangerReference.SetTimescale(0.1f);

				if (Input.GetMouseButtonUp(0))
				{
					timescaleChangerReference.SetTimescale(timescaleChangerReference.Level);
					FinalizeBuildingPlacement(colliders[0].GetComponent<BuildingAnchor>());
					return;
				}
			}

			if (Input.GetMouseButtonUp(1))
			{
				CancelBuildingPlacement();
			}
		}
	}

	private void Update()
	{
		EnableButtons();
		HandlePlacement();
	}
}
