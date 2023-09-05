using UnityEngine;

public class AutoDestroyAfterTime : MonoBehaviour
{
	[SerializeField] private float time = 1.0f;
	private float startTime;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time >= startTime + time)
		{
			Destroy(gameObject);
		}
	}
}
