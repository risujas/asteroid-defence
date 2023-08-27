using UnityEngine;

public class Attractor : MonoBehaviour
{
	private const float G = 0.01f;

	private Rigidbody rigidBody;

	private void AttractAttractables()
	{
		foreach (var a in Attractable.SpawnedAttractables)
		{
			var attractableRB = a.GetComponent<Rigidbody>();

			Vector3 direction = transform.position - a.transform.position;
			float distance = direction.magnitude;
			direction.Normalize();

			Vector3 force = direction * G * (rigidBody.mass * attractableRB.mass) / (distance * distance);
			attractableRB.AddForce(force * Time.deltaTime, ForceMode.Acceleration);
		}
	}

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		AttractAttractables();
	}
}
