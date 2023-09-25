using UnityEngine;

public class ExplosiveForce : MonoBehaviour
{
	[SerializeField] private GameObject origin = null;
	[SerializeField] private float force = 1.0f;
	[SerializeField] private float radius = 0.5f;

	public void ApplyWithinRadius()
	{
		if (origin == null)
		{
			origin = gameObject;
		}

		var affectedBodies = Physics.OverlapSphere(origin.transform.position, radius);
		foreach (var b in affectedBodies)
		{
			Vector3 direction = origin.transform.position - b.transform.position;
			float distance = direction.magnitude;
			direction.Normalize();

			float actualForce = force * (radius - distance) / radius;
			Vector3 forceVector = -direction * actualForce;

			b.attachedRigidbody.AddForce(forceVector, ForceMode.Impulse);
		}
	}
}
