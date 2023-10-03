using UnityEngine;

public class ImpactDamage : MonoBehaviour
{
	[SerializeField] private float damageMultiplier = 20.0f;
	private GravityBody gravityBody;

	public void Apply(Collision collision)
	{
		float colliderSize = GetComponent<Collider>().bounds.size.magnitude;
		float power = collision.relativeVelocity.magnitude * gravityBody.rb.mass;
		float radius = Mathf.Clamp(colliderSize * power, colliderSize, colliderSize * 2.0f);

		var affectedBodies = Physics.OverlapSphere(transform.position, radius);
		foreach (var b in affectedBodies)
		{
			if (b.gameObject == gameObject)
			{
				continue;
			}

			if (b.TryGetComponent(out Health health))
			{
				health.ChangeHealth(-power * damageMultiplier);
			}
		}
	}

	private void Start()
	{
		gravityBody = GetComponent<GravityBody>();
	}
}
