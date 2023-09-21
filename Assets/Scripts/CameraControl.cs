using UnityEngine;

public class CameraControl : MonoBehaviour
{
	[SerializeField] private float targetSize;
	[SerializeField] private float minTargetSize = 1.0f;
	[SerializeField] private float maxTargetSize = 10.0f;

	private float zoomSpeed = 3.0f;
	private float zoomSmoothingSpeed = 6.0f;

	private void HandleZoom()
	{
		var scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll != 0.0f)
		{
			targetSize -= scroll * zoomSpeed;
			targetSize = Mathf.Clamp(targetSize, minTargetSize, maxTargetSize);
		}

		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, zoomSmoothingSpeed * Time.unscaledDeltaTime);
	}

	private void Start()
	{
		targetSize = Camera.main.orthographicSize;
	}

	private void Update()
	{
		HandleZoom();
	}

	private void OnValidate()
	{
		targetSize = Mathf.Clamp(targetSize, minTargetSize, maxTargetSize);
		Camera.main.orthographicSize = targetSize;
	}
}
