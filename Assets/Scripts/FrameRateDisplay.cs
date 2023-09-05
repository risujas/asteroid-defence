using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameRateDisplay : MonoBehaviour
{
	private float updateInterval = 0.1f;
	private float lastUpdate = 0.0f;

	private List<float> fpsCounts = new();

	[SerializeField] private TextMeshProUGUI textMesh;

	private void Update()
	{
		if (Time.unscaledTime > lastUpdate + updateInterval)
		{
			float currentFps = 1.0f / Time.unscaledDeltaTime;
			fpsCounts.Add(currentFps);

			if (fpsCounts.Count > 10)
			{
				fpsCounts.RemoveAt(0);
			}

			float avgFps = 0.0f;
			foreach (var i in fpsCounts)
			{
				avgFps += i;
			}
			avgFps /= fpsCounts.Count;

			string text = "FPS: " + avgFps.ToString();
			textMesh.text = text;

			lastUpdate = Time.unscaledTime;
		}
	}
}
