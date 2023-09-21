using UnityEngine;

public class VelocityIndicator : MonoBehaviour
{
	[SerializeField] private Rigidbody trackedRb;
	[SerializeField] private float leadingTime = 20.0f;

	private LineRenderer lineRenderer;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		Vector3 velocityPoint = transform.position + trackedRb.velocity * leadingTime;

		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, velocityPoint);
	}
}
