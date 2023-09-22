using System.Collections;
using UnityEngine;

public class FadeLight : MonoBehaviour
{
	[SerializeField] private float fadeDuration = 0.5f;
	[SerializeField] private float targetIntensity = 0.0f;

	private Light fadeLight;
	private float startIntensity;

	private IEnumerator Fade()
	{
		fadeLight = GetComponent<Light>();
		startIntensity = fadeLight.intensity;

		float remaining = fadeDuration;
		while (remaining > 0.0f)
		{
			float timeStep = Time.deltaTime;
			if (timeStep > remaining)
			{
				timeStep = remaining;
			}
			remaining -= timeStep;

			float t = (fadeDuration - remaining) / fadeDuration;
			fadeLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);

			yield return null;
		}

		yield return null;
	}

	private void Start()
	{
		StartCoroutine(Fade());
	}
}
