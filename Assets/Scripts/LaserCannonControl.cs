using UnityEngine;

public class LaserCannonControl : MonoBehaviour
{
	[SerializeField] private LayerMask laserCollisionLayers;
	[SerializeField] private LayerMask laserForceAlterableLayers;

	[SerializeField] private Transform laserOrigin;
	[SerializeField] private GameObject laserFiringEffect;
	[SerializeField] private GameObject laserImpact;

	private const float laserBatteryPowerCap = 1.0f;
	[SerializeField] private float laserPower = 1.0f;
	[SerializeField] private float laserBatteryPower = laserBatteryPowerCap;
	[SerializeField] private float laserBatteryRechargeRate = 0.05f;
	[SerializeField] private float laserBatteryDepletionRate = 0.1f;

	private LineRenderer lineRenderer;

	private bool laserIsFiring = false;
	private bool laserHitAnObject;
	private RaycastHit laserHit;
	private Vector3 laserDir;

	private void RotateTurret()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;
		transform.up = (mousePos - transform.position).normalized;
	}

	private void DisableLaser()
	{
		laserIsFiring = false;
		lineRenderer.enabled = false;
		laserFiringEffect.SetActive(false);
		laserImpact.SetActive(false);
	}

	private void HandleLaserInput()
	{
		if (Input.GetMouseButtonDown(0) && laserBatteryPower > 0.0f)
		{
			laserIsFiring = true;

			lineRenderer.enabled = true;
			if (!laserFiringEffect.activeSelf)
			{
				laserFiringEffect.SetActive(true);
			}
		}
		else if (!Input.GetMouseButton(0) && laserIsFiring)
		{
			DisableLaser();
		}
	}

	private void HandleLaserBattery()
	{
		if (laserIsFiring)
		{
			laserBatteryPower -= laserBatteryDepletionRate * Time.deltaTime;

			if (laserBatteryPower < 0.0f)
			{
				DisableLaser();
			}
		}

		laserBatteryPower += laserBatteryRechargeRate * Time.deltaTime;
		laserBatteryPower = Mathf.Clamp(laserBatteryPower, 0.0f, laserBatteryPowerCap);
	}

	private void HandleLaserRaycast()
	{
		laserHitAnObject = false;

		if (laserIsFiring)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = transform.position.z;
			laserDir = (mousePos - laserOrigin.position).normalized;

			Ray ray = new Ray();
			ray.origin = laserOrigin.position;
			ray.direction = laserDir;

			if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, laserCollisionLayers))
			{
				laserHitAnObject = true;
				laserHit = hit;
			}
			else
			{
				laserImpact.SetActive(false);
			}
		}
	}

	private void HandleLaserVisual()
	{
		if (laserIsFiring)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = transform.position.z;

			Vector3 laserEndPoint = laserOrigin.position;
			laserEndPoint += laserDir * 1000.0f;

			if (laserHitAnObject)
			{
				laserEndPoint = laserHit.point;

				if (!laserImpact.activeSelf)
				{
					laserImpact.SetActive(true);
				}
				laserImpact.transform.position = laserHit.point + laserHit.normal * 0.05f;
			}
			else
			{
				laserImpact.SetActive(false);
			}

			lineRenderer.SetPosition(0, laserOrigin.position);
			lineRenderer.SetPosition(1, laserEndPoint);
		}
	}

	private void HandleLaserImpactForce(RaycastHit hit)
	{
		if (laserIsFiring && laserHitAnObject)
		{
			if ((laserForceAlterableLayers & (1 << hit.transform.gameObject.layer)) != 0)
			{
				var rb = hit.transform.GetComponent<Rigidbody>();

				Vector3 dir = (hit.transform.position - laserOrigin.position).normalized;
				Vector3 force = dir * laserPower;

				rb.AddForceAtPosition(force, hit.point, ForceMode.Force);
			}
		}
	}

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void OnDisable()
	{
		DisableLaser();
	}

	private void Update()
	{
		RotateTurret();

		HandleLaserInput();
		HandleLaserRaycast();
		HandleLaserBattery();
		HandleLaserVisual();
	}

	private void FixedUpdate()
	{
		HandleLaserImpactForce(laserHit);
	}
}
