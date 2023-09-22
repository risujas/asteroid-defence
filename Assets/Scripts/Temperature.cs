using UnityEngine;

public class Temperature : MonoBehaviour
{
	[SerializeField] private SunMotion solarSeason;

	private Material material;

	void Start()
	{
		material = GetComponent<Renderer>().material;
	}

	void Update()
	{
		float summerness = Mathf.Abs(0.5f - solarSeason.YearProgress) * 2.0f;
		float icecapExtent = Mathf.Lerp(0.4f, 0.1f, summerness);
		material.SetFloat("_IceCapCoverage", icecapExtent);
	}
}
