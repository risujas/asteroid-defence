using UnityEngine;

public class PlacementManager : MonoBehaviour
{
	[SerializeField] private FundsManager fundsManager;

	[Header("Placement Aid")]
	[SerializeField] private Material placementAidMaterialValid;
	[SerializeField] private Material placementAidMaterialInvalid;
	[SerializeField] private LayerMask snapLayerMask;
	[SerializeField] private TimescaleChanger timescaleChangerReference;
	[SerializeField] private LineRenderer placementLine;

	private bool isPlacing = false;
	private Placeable selectedPlaceablePrefab = null;
	private GameObject placementAidMarker = null;
	private Renderer placementAidMarkerRenderer = null;
	private bool placementEnablementDelay = false;

	public bool IsPlacing => isPlacing;

	public void StartPlacement(Placeable placeable)
	{
		placementEnablementDelay = true;
		selectedPlaceablePrefab = placeable;
		isPlacing = true;

		if (placementAidMarker != null)
		{
			Destroy(placementAidMarker);
		}

		placementAidMarker = Instantiate(selectedPlaceablePrefab, Vector3.zero, selectedPlaceablePrefab.transform.rotation).gameObject;
		placementAidMarker.transform.parent = transform;
		Destroy(placementAidMarker.GetComponent<Placeable>());
		placementAidMarkerRenderer = placementAidMarker.GetComponent<Renderer>();
	}

	private void FinalizePlacement(PlacementAnchor anchor)
	{
		fundsManager.Funds -= selectedPlaceablePrefab.PlacementCost;

		var newBuilding = Instantiate(selectedPlaceablePrefab, placementAidMarker.transform.position, placementAidMarker.transform.rotation);
		if (newBuilding.AddToAnchorHierarchy)
		{
			newBuilding.transform.parent = anchor.transform;
		}

		newBuilding.placementEvent.Invoke();

		if (newBuilding.PlacementEffect != null)
		{
			var placementEffect = Instantiate(newBuilding.PlacementEffect, placementAidMarker.transform.position, placementAidMarker.transform.rotation);
			placementEffect.transform.parent = anchor.transform;
		}

		if (!newBuilding.AllowMultiPlacement)
		{
			CancelPlacement();
		}
	}

	public void CancelPlacement()
	{
		isPlacing = false;
		selectedPlaceablePrefab = null;

		timescaleChangerReference.SetTimescale(timescaleChangerReference.Level);

		Destroy(placementAidMarker);
	}

	private void HandlePlacement()
	{
		if (isPlacing && !placementEnablementDelay)
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			var colliders = Physics.OverlapSphere(mousePos, 0.1f, snapLayerMask);
			if (colliders == null || colliders.Length == 0)
			{
				placementAidMarker.transform.position = mousePos;
				placementAidMarker.transform.up = Vector3.up;
				placementAidMarkerRenderer.material = placementAidMaterialInvalid;
				timescaleChangerReference.SetTimescale(timescaleChangerReference.Level);
			}
			else
			{
				var anchor = colliders[0].GetComponent<PlacementAnchor>();
				Vector2 dir = (mousePos - (Vector2)anchor.transform.position).normalized;

				placementLine.SetPosition(0, anchor.transform.position);
				placementLine.SetPosition(1, mousePos);

				placementAidMarker.transform.position = dir * anchor.SpawnHeight;
				placementAidMarker.transform.up = dir;

				if (selectedPlaceablePrefab.SlowDownWhilePlacing)
				{
					timescaleChangerReference.SetTimescale(0.1f);
				}

				if (fundsManager.Funds < selectedPlaceablePrefab.PlacementCost)
				{
					placementAidMarkerRenderer.material = placementAidMaterialInvalid;
				}
				else
				{
					placementAidMarkerRenderer.material = placementAidMaterialValid;

					if (Input.GetMouseButtonUp(0) || (selectedPlaceablePrefab.PlaceOnMouseDown && Input.GetMouseButtonDown(0)))
					{
						timescaleChangerReference.SetTimescale(timescaleChangerReference.Level);
						FinalizePlacement(colliders[0].GetComponent<PlacementAnchor>());
						return;
					}
				}
			}
		}
	}

	private void Start()
	{
		if (fundsManager == null)
		{
			fundsManager = GameObject.FindWithTag("FundsManager").GetComponent<FundsManager>();
		}
	}

	private void Update()
	{
		HandlePlacement();

		placementLine.enabled = isPlacing;
	}

	private void LateUpdate()
	{
		placementEnablementDelay = false;
	}
}
