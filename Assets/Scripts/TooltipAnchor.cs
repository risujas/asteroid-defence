using UnityEngine;

public class TooltipAnchor : MonoBehaviour
{
	[SerializeField] private LayerMask tooltipLayerMask;
	[SerializeField] private GameObject tooltipPrefab;

	private GameObject tooltipCanvas;
	private GameObject tooltipObject;

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

	private void PositionTooltip()
	{
		var anchorRatio = GetAnchorScreenRatio();

		Vector2 tooltipRatio = Vector2.zero;

		Vector3 tooltipScreenPos = Vector3.zero;
		tooltipScreenPos.x = Screen.width * tooltipRatio.x;
		tooltipScreenPos.y = Screen.height * tooltipRatio.y;

		tooltipObject.GetComponent<RectTransform>().position = tooltipScreenPos;
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

		sphereCollider = GetComponent<SphereCollider>();
		sphereSize = (sphereCollider.transform.lossyScale * sphereCollider.radius * 2).x;
	}

	private void Update()
	{
		if (Time.time > lastCheck + checkFrequency)
		{
			bool wasActive = tooltipObject.activeSelf;
			bool drawTooltip = CheckForMouseHover();
			tooltipObject.SetActive(drawTooltip);

			if (!wasActive && drawTooltip)
			{
				PositionTooltip();
			}
		}
	}
}
