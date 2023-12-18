using UnityEngine;

public class LaserCannonControl : MonoBehaviour
{
	[SerializeField] private LayerMask laserCollisionLayers;
	[SerializeField] private Transform laserOrigin;
	[SerializeField] private GameObject laserFiringEffect;

	private LineRenderer lineRenderer;

	private void RotateTurret()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;
		transform.up = (mousePos - transform.position).normalized;
	}

	private void SetLinePositions()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;

		Ray ray = new Ray();
		ray.origin = laserOrigin.position;
		ray.direction = (mousePos - laserOrigin.position).normalized;

		Vector3 laserEndPoint = laserOrigin.position;
		laserEndPoint += ray.direction * 1000.0f;

		if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, laserCollisionLayers))
		{
			laserEndPoint = hit.point;
		}

		lineRenderer.SetPosition(0, laserOrigin.position);
		lineRenderer.SetPosition(1, laserEndPoint);
	}

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void OnDisable()
	{
		lineRenderer.enabled = false;
		laserFiringEffect.SetActive(false);
	}

	private void Update()
	{
		RotateTurret();

		if (Input.GetMouseButton(0))
		{
			lineRenderer.enabled = true;
			SetLinePositions();

			if (!laserFiringEffect.activeSelf)
			{
				laserFiringEffect.SetActive(true);
			}
		}
		else
		{
			lineRenderer.enabled = false;
			laserFiringEffect.SetActive(false);
		}
	}
}
