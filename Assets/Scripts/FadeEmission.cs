using System.Collections;
using UnityEngine;

public class FadeEmission : MonoBehaviour
{
	[SerializeField] private Material material = null;
	[SerializeField] private float fadeDuration = 1.0f;
	[SerializeField] private float targetIntensity = 0.0f;
	[SerializeField] private string shaderFloatParameter = "_EmissionIntensity";

	private IEnumerator Fade()
	{
		float startIntensity = material.GetFloat(shaderFloatParameter);

		float elapsed = 0.0f;
		while (elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.Clamp01(elapsed / fadeDuration);
			material.SetFloat(shaderFloatParameter, Mathf.Lerp(startIntensity, targetIntensity, t));
			yield return null;
		}

		yield return null;
	}

	private void Start()
	{
		if (material == null)
		{
			material = GetComponent<Renderer>().material;
		}

		StartCoroutine(Fade());
	}
}
