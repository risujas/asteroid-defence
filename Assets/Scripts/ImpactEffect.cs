using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
	[SerializeField] private float length = 0.5f;

	private Light impactLight;
	private float startIntensity;

	private void Start()
	{
		impactLight = GetComponent<Light>();
		startIntensity = impactLight.intensity;
	}

	private void Update()
	{
		float intensityChange = startIntensity / length;
		impactLight.intensity -= intensityChange * Time.deltaTime;

		if (impactLight.intensity <= 0.0f)
		{
			Destroy(gameObject);
		}
	}
}
