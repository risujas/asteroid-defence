using System.Collections;
using UnityEngine;

public class FragmentTrail : MonoBehaviour
{
	[SerializeField] private SpriteRenderer fragmentRenderer;

	private TrailRenderer trailRenderer;
	private Material fragmentMaterial;
	private Material trailMaterial;

	private bool doingEmissionLerp = false;
	private Coroutine emissionLerp = null;

	private GameObject spawnedObjectsContainer;

	public void DetachTrailFromParent()
	{
		transform.parent = spawnedObjectsContainer.transform;

		var autoDestroy = gameObject.AddComponent<AutoDestroyAfterTime>();
		autoDestroy.Duration = trailRenderer.time;

		if (doingEmissionLerp)
		{
			StopCoroutine(emissionLerp);
		}

		StartCoroutine(AlphaLerp(trailRenderer.time));
	}

	private IEnumerator EmissionLerp()
	{
		doingEmissionLerp = true;

		float startEmissionIntensity = fragmentMaterial.GetFloat("_EmissionIntensity");
		float emissionRatio = 0.0f;

		Color fragmentMainColor = fragmentMaterial.GetColor("_MainColor");
		Color fragmentEmissionColor = fragmentMaterial.GetColor("_Emission");

		while (emissionRatio < 0.99f)
		{
			float currentEmissionIntensity = fragmentMaterial.GetFloat("_EmissionIntensity");
			emissionRatio = (startEmissionIntensity - currentEmissionIntensity) / startEmissionIntensity;

			Color trailStartColor = Color.Lerp(fragmentEmissionColor, fragmentMainColor, emissionRatio);
			trailStartColor.a = 0.05f;

			Color trailEndColor = trailStartColor;
			trailEndColor.a = 0.0f;

			trailMaterial.SetColor("_StartColor", trailStartColor);
			trailMaterial.SetColor("_EndColor", trailEndColor);

			yield return null;
		}

		doingEmissionLerp = false;
		yield return null;
	}

	private IEnumerator AlphaLerp(float duration)
	{
		Color initialColor = trailMaterial.GetColor("_StartColor");
		Color finalColor = initialColor;
		finalColor.a = 0.0f;

		float elapsed = 0.0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float ratio = Mathf.Clamp01(elapsed / duration);

			Color currentColor = Color.Lerp(initialColor, finalColor, ratio);
			trailMaterial.SetColor("_StartColor", currentColor);

			yield return null;
		}

		yield return null;
	}

	private void Start()
	{
		spawnedObjectsContainer = GameObject.FindWithTag("SpawnedObjectsContainer");

		fragmentMaterial = fragmentRenderer.material;
		trailRenderer = GetComponent<TrailRenderer>();
		trailMaterial = trailRenderer.material;

		emissionLerp = StartCoroutine(EmissionLerp());
	}
}
