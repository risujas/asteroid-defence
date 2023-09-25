using UnityEngine;

public class DamageWithinRadius : MonoBehaviour
{
	[SerializeField] private GameObject origin = null;
	[SerializeField] private float damage = 1.0f;
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
			if (b.TryGetComponent(out Health health))
			{
				health.ChangeHealth(-damage);
			}
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
