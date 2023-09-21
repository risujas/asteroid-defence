using UnityEngine;

public class ImpactDamage : MonoBehaviour
{
	private float radius = 0.2f;
	private float damageMultiplier = 2.0f;
	private Rigidbody rb;

	private void DamageNearbyHealthObjects()
	{
		float damage = rb.mass * rb.velocity.magnitude * damageMultiplier;
		foreach (var h in Health.HealthObjects)
		{
			float distance = Vector3.Distance(transform.position, h.transform.position);
			if (distance <= radius)
			{
				float effectiveDamage = damage * (1 - distance / radius);
				h.ChangeHealth(-effectiveDamage);
			}
		}
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		DamageNearbyHealthObjects();
	}
}
