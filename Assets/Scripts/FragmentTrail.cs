using UnityEngine;

public class FragmentTrail : MonoBehaviour
{
	[SerializeField] private SpriteRenderer fragmentRenderer;

	private TrailRenderer trailRenderer;
	private Material fragmentMaterial;
	private Material trailMaterial;

	private float startEmissionIntensity;
	private float currentEmissionIntensity;

	private Color fragmentMainColor;
	private Color fragmentEmissionColor;

	private bool lerpFinished = false;

	public void DetachTrailFromParent()
	{
		transform.parent = null;

		var autoDestroy = gameObject.AddComponent<AutoDestroyAfterTime>();
		autoDestroy.Duration = trailRenderer.time;
	}

	private void Start()
	{
		fragmentMaterial = fragmentRenderer.material;
		trailRenderer = GetComponent<TrailRenderer>();
		trailMaterial = trailRenderer.material;

		startEmissionIntensity = fragmentMaterial.GetFloat("_EmissionIntensity");

		fragmentMainColor = fragmentMaterial.GetColor("_MainColor");
		fragmentEmissionColor = fragmentMaterial.GetColor("_Emission");
	}

	private void Update()
	{
		if (!lerpFinished)
		{
			currentEmissionIntensity = fragmentMaterial.GetFloat("_EmissionIntensity");

			float emissionRatio = (startEmissionIntensity - currentEmissionIntensity) / startEmissionIntensity;

			Color trailStartColor = Color.Lerp(fragmentEmissionColor, fragmentMainColor, emissionRatio);
			trailStartColor.a = 0.2f;

			Color trailEndColor = trailStartColor;
			trailEndColor.a = 0.0f;

			trailMaterial.SetColor("_StartColor", trailStartColor);
			trailMaterial.SetColor("_EndColor", trailEndColor);

			if (emissionRatio > 0.99f)
			{
				lerpFinished = true;
			}
		}
	}
}
