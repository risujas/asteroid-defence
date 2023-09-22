using UnityEngine;

public class ImpactDamage
{
	private static float radius = 0.2f;
	private static float damageMultiplier = 2.0f;
	public static void DamageNearbyHealthObjects(Rigidbody rb)
	{
		float damage = rb.mass * rb.velocity.magnitude * damageMultiplier;
		foreach (var h in Health.HealthObjects)
		{
			float distance = Vector3.Distance(rb.transform.position, h.transform.position);
			if (distance <= radius)
			{
				float effectiveDamage = damage * (1 - distance / radius);
				h.ChangeHealth(-effectiveDamage);
			}
		}
	}
}
