using System.Collections;
using UnityEngine;

public class UiGroupFader : MonoBehaviour
{
	[SerializeField] private CanvasGroup defeatCanvasGroup;
	[SerializeField] private CanvasGroup gameRuntimeUiCanvasGroup;
	[SerializeField] private float fadeDuration = 0.25f;

	public void SwitchToDefeatScreen()
	{
		StartCoroutine(FadeIn(defeatCanvasGroup, fadeDuration));
		StartCoroutine(FadeOut(gameRuntimeUiCanvasGroup, fadeDuration));
	}

	public void SwitchToRuntimeUi()
	{
		StartCoroutine(FadeIn(gameRuntimeUiCanvasGroup, fadeDuration));
		StartCoroutine(FadeOut(defeatCanvasGroup, fadeDuration));
	}

	private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
	{
		canvasGroup.gameObject.SetActive(true);
		canvasGroup.alpha = 0.0f;

		float elapsed = 0.0f;
		while (elapsed < duration)
		{
			elapsed += Time.unscaledDeltaTime;
			float t = Mathf.Clamp01(elapsed / duration);
			canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, t);
			yield return null;
		}

		yield return null;
	}

	private IEnumerator FadeOut(CanvasGroup canvasGroup, float duration)
	{
		float initialAlpha = canvasGroup.alpha;

		float elapsed = 0.0f;
		while (elapsed < duration)
		{
			elapsed += Time.unscaledDeltaTime;
			float t = Mathf.Clamp01(elapsed / duration);
			canvasGroup.alpha = Mathf.Lerp(initialAlpha, 0.0f, t);
			yield return null;
		}

		canvasGroup.gameObject.SetActive(false);

		yield return null;
	}
}
