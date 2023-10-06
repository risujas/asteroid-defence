using System.Collections;
using UnityEngine;

public class UiGroupFader : MonoBehaviour
{
	[SerializeField] private CanvasGroup defeatCanvasGroup;
	[SerializeField] private CanvasGroup gameRuntimeUiCanvasGroup;

	public void SwitchToDefeatScreen()
	{
		StartCoroutine(FadeIn(defeatCanvasGroup, 0.5f));
		StartCoroutine(FadeOut(gameRuntimeUiCanvasGroup, 0.5f));
	}

	public void SwitchToRuntimeUi()
	{
		StartCoroutine(FadeIn(gameRuntimeUiCanvasGroup, 0.5f));
		StartCoroutine(FadeOut(defeatCanvasGroup, 0.5f));
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
