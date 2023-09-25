using UnityEngine;

public class ExplosiveForceWithinRadius : MonoBehaviour
{
	[SerializeField] private GameObject origin = null;
	[SerializeField] private float force = 1.0f;
	[SerializeField] private float radius = 0.5f;
	[SerializeField] private bool autoApply = false;
	[SerializeField] private int autoApplyFrameDelay = 1;

	private int initialFrame = 0;

	public void Apply()
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

	private void Start()
	{
		initialFrame = Time.frameCount;
	}

	private void Update()
	{
		if (autoApply && Time.frameCount >= initialFrame + autoApplyFrameDelay)
		{
			autoApply = false;
			Apply();
		}
	}
}
