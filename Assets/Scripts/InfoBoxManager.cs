using UnityEngine;

public class InfoBoxManager : MonoBehaviour
{
	[SerializeField] private LayerMask infoBoxLayerMask;
	[SerializeField] private Canvas infoBoxCanvas;
	[SerializeField] private GameObject infoBox;

	private InfoBoxTrigger previousTrigger;
	private BuildingPlacer buildingPlacer;

	private void CheckForInput()
	{
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, infoBoxLayerMask))
			{
				var selectedTrigger = hit.transform.GetComponent<InfoBoxTrigger>();

				if (previousTrigger == selectedTrigger)
				{
					Destroy(infoBox);
					previousTrigger = null;
					return;
				}

				if (infoBox != null)
				{
					Destroy(infoBox);
					previousTrigger = null;
				}

				infoBox = Instantiate(selectedTrigger.InfoBoxPrefab, selectedTrigger.InfoBoxPrefab.transform.position, selectedTrigger.InfoBoxPrefab.transform.rotation, infoBoxCanvas.transform);

				var rectTransform = infoBox.GetComponent<RectTransform>();
				rectTransform.position = new Vector2(Screen.width / 2.0f, 0.0f);

				previousTrigger = selectedTrigger;
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
