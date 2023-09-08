using System.Collections;
using UnityEngine;

public class FadeEmission : MonoBehaviour
{
	[SerializeField] private float fadeDuration = 1.0f;
	[SerializeField] private float targetIntensity = 0.0f;

	private Material material;

	private IEnumerator Fade()
	{
		float elapsed = 0.0f;

		float startIntensity = material.GetFloat("_EmissionIntensity");

		while (elapsed <= fadeDuration)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / fadeDuration;
			material.SetFloat("_EmissionIntensity", Mathf.Lerp(startIntensity, targetIntensity, t));
			yield return null;
		}

		yield return null;
	}

	private void Start()
	{
		material = GetComponent<Renderer>().material;

		StartCoroutine(Fade());
	}
}
