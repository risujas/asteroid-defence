using UnityEngine;

public class TooltipAnchor : MonoBehaviour
{
	[SerializeField] private GameObject tooltipPrefab;

	private GameObject tooltipCanvas;

	private GameObject tooltipObject;
	private RectTransform tooltipObjectRectTransform;

	private LineRenderer tooltipTopLine;
	private LineRenderer tooltipBottomLine;
	private LineRenderer activeLine;

	private Vector2Int offsetAxes = new Vector2Int();
	private Vector3 screenSpaceOffset = Vector3.zero;

	private SphereCollider sphereCollider;
	private float sphereSize;

	public void Toggle()
	{
		tooltipObject.SetActive(!tooltipObject.activeSelf);

		ChooseOffsetAxes();
	}

	private void ChooseOffsetAxes()
	{
		var anchorRatio = GetAnchorScreenRatio();

		offsetAxes.x = anchorRatio.x <= 0.5f ? 1 : -1;
		offsetAxes.y = anchorRatio.y <= 0.5f ? 1 : -1;
	}

	private void CalculateOffset()
	{
		var anchorRatio = GetAnchorScreenRatio();

		Vector3 tooltipScreenPos = Vector3.zero;
		tooltipScreenPos.x = Screen.width * anchorRatio.x;
		tooltipScreenPos.y = Screen.height * anchorRatio.y;

		Vector3 adjustedScreenPos = tooltipScreenPos;

		Vector3 tooltipWorldPos = Camera.main.ScreenToWorldPoint(adjustedScreenPos);
		tooltipWorldPos.y += sphereSize / 2.0f * offsetAxes.y;

		adjustedScreenPos = Camera.main.WorldToScreenPoint(tooltipWorldPos);
		adjustedScreenPos.y += tooltipObjectRectTransform.sizeDelta.y * offsetAxes.y;

		screenSpaceOffset = adjustedScreenPos - tooltipScreenPos;
	}

	private void SetTooltipPosition()
	{
		var anchorRatio = GetAnchorScreenRatio();

		Vector3 tooltipScreenPos = Vector3.zero;
		tooltipScreenPos.x = Screen.width * anchorRatio.x;
		tooltipScreenPos.y = Screen.height * anchorRatio.y;

		tooltipObjectRectTransform.position = tooltipScreenPos + screenSpaceOffset;
	}

	private void SetLinesEnabled()
	{
		var anchorRatio = GetAnchorScreenRatio();

		tooltipTopLine.gameObject.SetActive(false);
		tooltipBottomLine.gameObject.SetActive(false);

		activeLine = offsetAxes.y > 0 ? tooltipBottomLine : tooltipTopLine;
		activeLine.gameObject.SetActive(true);
	}

	private void ChooseLine()
	{
		Vector3 tooltipWorldPos = Camera.main.ScreenToWorldPoint(activeLine.transform.position);
		activeLine.SetPosition(0, new Vector3(tooltipWorldPos.x, tooltipWorldPos.y, 1.0f));
		activeLine.SetPosition(1, new Vector3(transform.position.x, transform.position.y, 1.0f));
	}

	private Vector2 GetAnchorScreenRatio()
	{
		Vector3 anchorScreenPos = Camera.main.WorldToScreenPoint(transform.position);

		Vector2 anchorRatio = Vector2.zero;
		anchorRatio.x = anchorScreenPos.x / Screen.width;
		anchorRatio.y = anchorScreenPos.y / Screen.height;

		return anchorRatio;
	}

	private void Start()
	{
		tooltipCanvas = GameObject.FindWithTag("MainCanvas");

		tooltipObject = Instantiate(tooltipPrefab, Vector3.zero, Quaternion.identity, tooltipCanvas.transform);
		tooltipObject.SetActive(false);
		tooltipObjectRectTransform = tooltipObject.GetComponentInChildren<RectTransform>();

		tooltipTopLine = tooltipObject.transform.Find("TopLine").GetComponent<LineRenderer>();
		tooltipBottomLine = tooltipObject.transform.Find("BottomLine").GetComponent<LineRenderer>();

		sphereCollider = GetComponent<SphereCollider>();
		sphereSize = (sphereCollider.transform.lossyScale * sphereCollider.radius * 2).x;
	}

	private void Update()
	{
		if (tooltipObject.activeSelf)
		{
			CalculateOffset();
			SetTooltipPosition();

			SetLinesEnabled();
			ChooseLine();
		}
	}
}
