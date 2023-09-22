using System.Collections;
using UnityEngine;

public class FadeLight : MonoBehaviour
{
	[SerializeField] private Light fadeLight = null;
	[SerializeField] private float fadeDuration = 0.5f;
	[SerializeField] private float targetIntensity = 0.0f;

	private float startIntensity;

	private IEnumerator Fade()
	{
		startIntensity = fadeLight.intensity;

		float elapsed = 0.0f;
		while (elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.Clamp01(elapsed / fadeDuration);
			fadeLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
			yield return null;
		}

		yield return null;
	}

	private void Start()
	{
		if (fadeLight == null)
		{
			fadeLight = GetComponent<Light>();
		}

		StartCoroutine(Fade());
	}
}
