using UnityEngine;

public class MissileControl : MonoBehaviour
{
	[SerializeField] private float maxDeltaV = 10.0f;
	[SerializeField] private float remainingDeltaV = 10.0f;
	[SerializeField] private float acceleration = 1.0f;

	private Rigidbody rb;

	private void HandleRotation()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;
		Vector3 direction = mousePos - transform.position;

		transform.up = direction.normalized;
	}

	private void HandleAcceleration()
	{
		if (remainingDeltaV > maxDeltaV)
		{
			remainingDeltaV = maxDeltaV;
		}

		if (Input.GetMouseButton(0))
		{
			if (remainingDeltaV > 0.0f)
			{
				float deltaV = acceleration * Time.deltaTime;
				if (remainingDeltaV < deltaV)
				{
					deltaV = remainingDeltaV;
				}

				remainingDeltaV -= deltaV;
				rb.AddForce(transform.up * deltaV, ForceMode.VelocityChange);
			}
		}
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		HandleRotation();
		HandleAcceleration();

		if (Input.GetMouseButtonUp(1))
		{
			enabled = false;
		}
	}
}
