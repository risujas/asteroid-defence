using UnityEngine;
public class CreateGameObject : MonoBehaviour
{
	[SerializeField] private Transform origin;
	[SerializeField] private GameObject selectedObject;
	[SerializeField] private bool enableTimer = false;
	[SerializeField] private float timerDuration = 1.0f;

	private float startTime = 0.0f;

	public void InstantiateSelected()
	{
		Instantiate(selectedObject, origin.position, origin.rotation);
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
				InstantiateSelected();
			}
		}
	}
}