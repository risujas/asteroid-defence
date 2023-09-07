using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	private const float minTargetSize = 1.0f;
	private const float maxTargetSize = 10.0f;
	private float targetSize;

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

	private void CenterOnObject()
	{
		Vector3 targetPosition = focusedObject.transform.position + new Vector3(0.0f, 0.0f, -5.0f);
		transform.position = Vector3.Lerp(transform.position, targetPosition, focusSwitchSpeed * Time.unscaledDeltaTime);
	}

	private void Start()
	{
		targetSize = Camera.main.orthographicSize;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			CycleFocusedObject(-1);
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			CycleFocusedObject(1);
		}

		var scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll != 0.0f)
		{
			targetSize -= scroll * zoomSpeed;
			targetSize = Mathf.Clamp(targetSize, minTargetSize, maxTargetSize);
		}

		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, zoomSmoothingSpeed * Time.unscaledDeltaTime);
	}

	private void LateUpdate()
	{
		if (focusedObject == null)
		{
			CycleFocusedObject(0);
		}

		CenterOnObject();
	}
}
