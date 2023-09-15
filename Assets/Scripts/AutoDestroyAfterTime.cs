using UnityEngine;

public class AutoDestroyAfterTime : MonoBehaviour
{
	[SerializeField] private float duration = 1.0f;
	private float startTime;

	public float Duration
	{
		get { return duration; }
		set { duration = value; }
	}

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time >= startTime + duration)
		{
			Destroy(gameObject);
		}
	}
}
