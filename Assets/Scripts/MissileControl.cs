using UnityEngine;

public class MissileControl : MonoBehaviour
{
	[SerializeField] private float acceleration = 1.0f;

	private Attractable attractable;

	private void Start()
	{
		attractable = GetComponent<Attractable>();
	}

	private void Update()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;
		Vector3 direction = mousePos - transform.position;

		transform.up = direction.normalized;

		if (Input.GetMouseButton(0))
		{
			Vector3 deltaV = transform.up * acceleration * Time.deltaTime;
			attractable.AddVelocity(deltaV);
		}
	}
}
