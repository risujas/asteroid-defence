using UnityEngine;

public class PlanetClimate : MonoBehaviour
{
	[SerializeField] private SunMotion sunMotion;

	[SerializeField] private float iceCapCoverage = 0.0f;
	[SerializeField] private float minCoverage = 0.1f;
	[SerializeField] private float maxCoverage = 0.4f;

	private Material material;

	private void SetIceCapCoverage(Material m)
	{
		iceCapCoverage = Mathf.Clamp(iceCapCoverage, minCoverage, maxCoverage);
		m.SetFloat("_IceCapCoverage", iceCapCoverage);
	}

	private void Start()
	{
		material = GetComponent<Renderer>().material;
	}

	private void Update()
	{
		float summerness = Mathf.Abs(0.5f - sunMotion.YearProgress) * 2.0f;

		iceCapCoverage = Mathf.Lerp(maxCoverage, minCoverage, summerness);
		SetIceCapCoverage(material);
	}

	private void OnValidate()
	{
		var m = GetComponent<Renderer>().sharedMaterial;
		SetIceCapCoverage(m);
	}
}
