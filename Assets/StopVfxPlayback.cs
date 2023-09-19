using UnityEngine;
using UnityEngine.VFX;

public class StopVfxPlayback : MonoBehaviour
{
	[SerializeField] private float duration = 1.0f;
	[SerializeField] private VisualEffect vfx = null;

	private float startTime;

	private void Start()
	{
		if (vfx != null)
		{
			vfx = GetComponent<VisualEffect>();
		}

		startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time > startTime + duration)
		{
			vfx.Stop();
			enabled = false;
		}
	}
}
