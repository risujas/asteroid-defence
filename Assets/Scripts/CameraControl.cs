using UnityEngine;

public class CameraControl : MonoBehaviour
{
	private const float minTargetSize = 1.0f;
	private const float maxTargetSize = 20.0f;
	private float targetSize;

	private float zoomSpeed = 2.0f;
	private float lerpSpeed = 3.0f;

	private void Start()
	{
		targetSize = Camera.main.orthographicSize;
	}

	private void Update()
	{
		var scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll != 0.0f)
		{
			targetSize -= scroll * zoomSpeed;
			targetSize = Mathf.Clamp(targetSize, minTargetSize, maxTargetSize);
		}

		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, lerpSpeed * Time.deltaTime);
	}
}
