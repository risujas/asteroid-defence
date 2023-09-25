using UnityEngine;
public class DestroyGameObject : MonoBehaviour
{
	[SerializeField] private GameObject selectedObject;
	[SerializeField] private bool enableTimer = false;
	[SerializeField] private float timerDuration = 1.0f;

	private float startTime = 0.0f;

	public void DestroySelected()
	{
		Destroy(selectedObject);
	}

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		if (enableTimer)
		{
			if (Time.time >= startTime + timerDuration)
			{
				DestroySelected();
			}
		}
	}
}