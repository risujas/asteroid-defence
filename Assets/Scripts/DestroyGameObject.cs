using UnityEngine;
public class DestroyGameObject : MonoBehaviour
{
	[SerializeField] private GameObject selectedObject;
	[SerializeField] private bool autoApply = false;
	[SerializeField] private float autoApplyDelay = 0.0f;

	private float initialTime = 0.0f;

	public void DestroySelected()
	{
		Destroy(selectedObject);
	}

	private void Start()
	{
		initialTime = Time.time;
	}

	private void Update()
	{
		if (autoApply && Time.time >= initialTime + autoApplyDelay)
		{
			DestroySelected();
		}
	}
}