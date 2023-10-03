using UnityEngine;

public class PlanetClimate : MonoBehaviour
{
	[SerializeField] private SunMotion sunMotion;

	[SerializeField] private float snowCoverage = 0.0f;
	[SerializeField] private float minCoverage = 0.1f;
	[SerializeField] private float maxCoverage = 0.4f;

	private Material material;

	private void SetIceCapCoverage(Material m)
	{
		snowCoverage = Mathf.Clamp(snowCoverage, minCoverage, maxCoverage);
		m.SetFloat("_SnowCoverage", snowCoverage);
	}

	private void Start()
	{
		material = GetComponent<Renderer>().material;
	}

	private void Update()
	{
		float summerness = Mathf.Abs(0.5f - sunMotion.YearProgress) * 2.0f;

		snowCoverage = Mathf.Lerp(maxCoverage, minCoverage, summerness);
		SetIceCapCoverage(material);
	}

	private void OnValidate()
	{
		var m = GetComponent<Renderer>().sharedMaterial;
		SetIceCapCoverage(m);
	}
}
