using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	[SerializeField] private float targetSize;
	[SerializeField] private float minTargetSize = 1.0f;
	[SerializeField] private float maxTargetSize = 10.0f;
	[SerializeField] private float keyboardSpeed = 3.0f;

	private float zoomSpeed = 3.0f;
	private float zoomSmoothingSpeed = 6.0f;
	private float focusSwitchSpeed = 6.0f;
	private int focusedObjectIndex = 0;
	private GameObject focusedObject;

	private void CycleFocusedObject(int change)
	{
		List<GameObject> objects = new();

		var sortedAttractors = Attractor.Attractors.OrderByDescending(a => a.Mass).ToList();
		foreach (var a in sortedAttractors)
		{
			objects.Add(a.gameObject);
		}

		focusedObjectIndex += change;
		if (focusedObjectIndex < 0)
		{
			focusedObjectIndex = objects.Count - 1;
		}
		if (focusedObjectIndex >= objects.Count)
		{
			focusedObjectIndex = 0;
		}

		focusedObject = objects[focusedObjectIndex];
	}

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

	private void HandleKeyboardMovement()
	{
		float effectiveSpeed = keyboardSpeed;
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			effectiveSpeed *= 2.0f;
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position += Vector3.left * effectiveSpeed * Time.unscaledDeltaTime;
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.position += Vector3.right * effectiveSpeed * Time.unscaledDeltaTime;
		}

		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.position += Vector3.up * effectiveSpeed * Time.unscaledDeltaTime;
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.position += Vector3.down * effectiveSpeed * Time.unscaledDeltaTime;
		}

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
		{
			focusedObject = null;
		}
	}

	private void CenterOnObject()
	{
		Vector3 targetPosition = focusedObject.transform.position + new Vector3(0.0f, 0.0f, -5.0f);
		transform.position = Vector3.Lerp(transform.position, targetPosition, focusSwitchSpeed * Time.unscaledDeltaTime);
	}

	private void Start()
	{
		targetSize = Camera.main.orthographicSize;

		CycleFocusedObject(0);
	}

	private void Update()
	{
		HandleKeyboardMovement();

		if (Input.GetKeyDown(KeyCode.Comma))
		{
			CycleFocusedObject(-1);
		}

		if (Input.GetKeyDown(KeyCode.Period))
		{
			CycleFocusedObject(1);
		}

		HandleZoom();

		if (focusedObject != null)
		{
			CenterOnObject();
		}
	}

	private void OnValidate()
	{
		targetSize = Mathf.Clamp(targetSize, minTargetSize, maxTargetSize);
		Camera.main.orthographicSize = targetSize;
	}
}
