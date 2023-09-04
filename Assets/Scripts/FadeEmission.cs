using System.Collections;
using UnityEngine;

public class FadeEmission : MonoBehaviour
{
	[SerializeField] private float fadeDuration = 1.0f;
	[SerializeField] private Color targetColor;

	private Material material;

	private IEnumerator Fade()
	{
		float elapsed = 0.0f;
		Color startColor = material.GetColor("_Color");

		while (elapsed <= fadeDuration)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / fadeDuration;
			material.SetColor("_Color", Color.Lerp(startColor, targetColor, t));
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
