using UnityEngine;

public class DelayedColliderEnablement : MonoBehaviour
{
	[SerializeField] private float delayTime;
	[SerializeField] private Collider colliderToEnable;

	private void EnableCollider()
	{
		colliderToEnable.enabled = true;
	}

	private void OnEnable()
	{
		Invoke("EnableCollider", delayTime);
	}
}
