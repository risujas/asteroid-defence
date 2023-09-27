using UnityEngine;

public class PlanetClimate : MonoBehaviour
{
	[SerializeField] private SunMotion sunMotion;

	private Material material;

	void Start()
	{
		material = GetComponent<Renderer>().material;
	}

	void Update()
	{
		float summerness = Mathf.Abs(0.5f - sunMotion.YearProgress) * 2.0f;
		float icecapExtent = Mathf.Lerp(0.4f, 0.1f, summerness);
		material.SetFloat("_IceCapCoverage", icecapExtent);
	}
}
