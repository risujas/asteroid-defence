using UnityEngine;

public class TooltipAnchor : MonoBehaviour
{
	[SerializeField] private LayerMask tooltipLayerMask;
	[SerializeField] private GameObject tooltipPrefab;

	private GameObject tooltipCanvas;

	private GameObject tooltipObject;
	private RectTransform tooltipObjectRectTransform;

	private LineRenderer tooltipTopLine;
	private LineRenderer tooltipBottomLine;
	private LineRenderer activeLine;

	private float checkFrequency = 1.0f;
	private float lastCheck = 0.0f;

	private SphereCollider sphereCollider;
	private float sphereSize;

	private bool CheckForMouseHover()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, tooltipLayerMask))
		{
			if (hit.transform.gameObject == gameObject)
			{
				return true;
			}
		}

		return false;
	}

	private void SetTooltipPosition()
	{
		var anchorRatio = GetAnchorScreenRatio();

		Vector3 tooltipScreenPos = Vector3.zero;
		tooltipScreenPos.x = Screen.width * anchorRatio.x;
		tooltipScreenPos.y = Screen.height * anchorRatio.y;

		Vector3 tooltipWorldPos = Camera.main.ScreenToWorldPoint(tooltipScreenPos);
		tooltipWorldPos.y += sphereSize / 2.0f;

		tooltipScreenPos = Camera.main.WorldToScreenPoint(tooltipWorldPos);
		tooltipScreenPos.y += tooltipObjectRectTransform.sizeDelta.y;

		tooltipObjectRectTransform.position = tooltipScreenPos;
	}

	private void SetLinePosition()
	{
		tooltipTopLine.gameObject.SetActive(false);
		tooltipBottomLine.gameObject.SetActive(false);

		var anchorRatio = GetAnchorScreenRatio();
		if (anchorRatio.y < 0.5f)
		{
			activeLine = tooltipBottomLine;
		}
		else
		{
			activeLine = tooltipTopLine;
		}

		activeLine.gameObject.SetActive(true);

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

	private void LateUpdate()
	{
		if (Time.time > lastCheck + checkFrequency)
		{
			bool wasActive = tooltipObject.activeSelf;
			bool drawTooltip = CheckForMouseHover();
			tooltipObject.SetActive(drawTooltip);

			if (!wasActive && drawTooltip)
			{
				SetTooltipPosition();
			}
		}

		if (tooltipObject.activeSelf)
		{
			SetLinePosition();
		}
	}
}
