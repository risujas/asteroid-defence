using UnityEngine;

public class DelayedColliderEnablement : MonoBehaviour
{
	[SerializeField] private float delayTime;
	[SerializeField] private Collider col;

	private void EnableComponent()
	{
		col.enabled = true;
	}

	private void OnEnable()
	{
		Invoke("EnableComponent", delayTime);
	}
}
