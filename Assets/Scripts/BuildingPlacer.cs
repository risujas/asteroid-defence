using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
	[SerializeField] private float buildPoints = 100.0f;

	[SerializeField] private Building FactoryPrefab;
	[SerializeField] private Building CommandCenterPrefab;

	[SerializeField] private Material placementAidMaterialValid;
	[SerializeField] private Material placementAidMaterialInvalid;

	[SerializeField] private LayerMask buildingSnappingLayerMask;
	[SerializeField] private TimescaleChanger timescaleChanger;

	private bool isPlacingBuilding = false;
	private Building selectedBuildingPrefab = null;
	private GameObject placementAidMarker = null;
	private Renderer placementAidMarkerRenderer = null;

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
		placementAidMarker.transform.parent = transform;
		Destroy(placementAidMarker.GetComponent<Building>());
		placementAidMarkerRenderer = placementAidMarker.GetComponent<Renderer>();
	}

	private void FinalizeBuildingPlacement(BuildingAnchor anchor)
	{
		var newBuilding = Instantiate(selectedBuildingPrefab, placementAidMarker.transform.position, placementAidMarker.transform.rotation);
		newBuilding.transform.parent = anchor.transform;

		CancelBuildingPlacement();
	}

	private void CancelBuildingPlacement()
	{
		isPlacingBuilding = false;
		selectedBuildingPrefab = null;

		Destroy(placementAidMarker);
	}

	private void Start()
	{
	}

	private void Update()
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
				timescaleChanger.SetTimescale(timescaleChanger.Level);
			}
			else
			{
				var anchor = colliders[0].GetComponent<BuildingAnchor>();
				Vector2 dir = (mousePos - (Vector2)anchor.transform.position).normalized;

				placementAidMarker.transform.position = dir * anchor.SpawnHeight;
				placementAidMarker.transform.up = dir;
				placementAidMarkerRenderer.material = placementAidMaterialValid;
				timescaleChanger.SetTimescale(0.1f);

				if (Input.GetMouseButtonUp(0))
				{
					timescaleChanger.SetTimescale(timescaleChanger.Level);
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
}
