using UnityEngine;

public class ExplosiveForce : MonoBehaviour
{
	[SerializeField] private GameObject origin = null;
	[SerializeField] private float force = 1.0f;
	[SerializeField] private float radius = 0.5f;

	public void Apply()
	{
		if (origin == null)
		{
			origin = gameObject;
		}

		var affectedBodies = Physics.OverlapSphere(origin.transform.position, radius);
		foreach (var b in affectedBodies)
		{
			b.attachedRigidbody.AddExplosionForce(force, origin.transform.position, radius);
		}
	}
}
