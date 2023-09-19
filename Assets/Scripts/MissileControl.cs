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
		if (Input.GetMouseButton(0))
		{
			Vector3 deltaV = transform.up * acceleration * Time.deltaTime;
			attractable.AddVelocity(deltaV);
		}
	}
}
