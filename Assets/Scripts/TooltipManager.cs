using UnityEngine;

public class TooltipManager : MonoBehaviour
{
	[SerializeField] private LayerMask tooltipLayerMask;

	private BuildingPlacer buildingPlacer;

	private void CheckForInput()
	{
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, tooltipLayerMask))
			{
				var anchor = hit.transform.GetComponent<TooltipAnchor>();
				anchor.Toggle();
			}
		}
	}

	private void Start()
	{
		buildingPlacer = GameObject.FindWithTag("BuildingPlacer").GetComponent<BuildingPlacer>();
	}

	private void Update()
	{
		bool tooltipsAllowed = !buildingPlacer.IsPlacingBuilding;

		if (tooltipsAllowed)
		{
			CheckForInput();
		}
	}
}
