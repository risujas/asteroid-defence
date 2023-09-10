using UnityEngine;

public class BuildingManager : MonoBehaviour
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
		Destroy(placementAidMarker.GetComponent<Building>());
		placementAidMarkerRenderer = placementAidMarker.GetComponent<Renderer>();
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
			if (colliders.Length > 0)
			{
				var anchor = colliders[0].GetComponent<BuildingAnchor>();
				Vector2 dir = (mousePos - (Vector2)anchor.transform.position).normalized;

				placementAidMarker.transform.position = dir * anchor.SpawnHeight;
				placementAidMarker.transform.up = dir;
				placementAidMarkerRenderer.material = placementAidMaterialValid;
				timescaleChanger.SetTimescale(0.1f);
			}
			else
			{
				placementAidMarker.transform.position = mousePos;
				placementAidMarker.transform.up = Vector3.up;
				placementAidMarkerRenderer.material = placementAidMaterialInvalid;
				timescaleChanger.SetTimescale(timescaleChanger.Level);
			}

			Utilities.DrawAxes(placementAidMarker.transform, 5.0f, Time.deltaTime);
		}
	}
}
