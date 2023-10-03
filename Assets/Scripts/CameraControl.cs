using UnityEngine;

public class CameraControl : MonoBehaviour
{
	private const float cameraZ = -5.0f;

	[SerializeField] private GameObject defaultFollowedObject;
	[SerializeField] private GameObject followedObject;

	[SerializeField] private float targetSize;
	[SerializeField] private float minTargetSize = 1.0f;
	[SerializeField] private float maxTargetSize = 10.0f;

	private float zoomSpeed = 3.0f;
	private float zoomSmoothingSpeed = 6.0f;

	private bool enableFollowObject = true;
	private float followSpeed = 2.0f;

	private bool enableFollowReset = true;
	private bool followReset = false;
	private float followResetTime = 1.0f;
	private float followResetStart = 0.0f;

	public GameObject FollowedObject { get { return followedObject; } set { followedObject = value; } }

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

	private void HandleFollowing()
	{
		if (enableFollowObject)
		{
			if (followedObject != null)
			{
				var nextPosition = Vector3.Lerp(transform.position, followedObject.transform.position, followSpeed * Time.unscaledDeltaTime);
				nextPosition.z = cameraZ;
				transform.position = nextPosition;
			}
			else if (enableFollowReset)
			{
				if (!followReset)
				{
					followReset = true;
					followResetStart = Time.time;
				}
				else if (Time.time >= followResetStart + followResetTime)
				{
					followReset = false;
					followedObject = defaultFollowedObject;
				}
			}
		}
	}

	private void Start()
	{
		targetSize = Camera.main.orthographicSize;

		if (defaultFollowedObject == null)
		{
			defaultFollowedObject = GameObject.FindWithTag("CentralBody");
		}
	}

	private void LateUpdate()
	{
		HandleZoom();
		HandleFollowing();
	}

	private void OnValidate()
	{
		targetSize = Mathf.Clamp(targetSize, minTargetSize, maxTargetSize);
		Camera.main.orthographicSize = targetSize;
	}
}
