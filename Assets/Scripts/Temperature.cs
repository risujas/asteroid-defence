using UnityEngine;

public class Temperature : MonoBehaviour
{
	[SerializeField] private Material material = null;
	[SerializeField] private Light temperatureLight;
	[SerializeField] private string materialEmissionString = "_EmissionIntensity";
	[SerializeField] private float maxEmissionIntensity = 10.0f;
	[SerializeField] private float temperature = 0.0f;

	private float coolingSpeed = 50.0f;
	private float minTemperature = 0.0f;
	private float maxTemperature = 1000.0f;
	private IntervalTimer adjustmentTimer = new IntervalTimer(1.0f / 30);

	public float MaxTemperature => maxTemperature;

	public void ChangeTemperature(float change)
	{
		temperature += change;
	}

	private void AdjustGlow()
	{
		float t = temperature / maxTemperature * maxEmissionIntensity;

		if (material != null)
		{
			if (material.HasFloat(materialEmissionString))
			{
				material.SetFloat(materialEmissionString, t);
			}
			else
			{
				Debug.Log("Shader material: " + material + " on object:" + gameObject.name + " doesn't contain the emission control float: " + materialEmissionString);
				enabled = false;
			}
		}

		if (temperatureLight != null)
		{
			temperatureLight.intensity = t;
		}
	}

	private void Start()
	{
		if (material == null)
		{
			material = GetComponent<Renderer>().material;
		}

		if (temperatureLight == null)
		{
			temperatureLight = GetComponent<Light>();
		}
	}

	private void Update()
	{
		if (temperature > 0.0f)
		{
			temperature -= coolingSpeed * Time.deltaTime;
			temperature = Mathf.Clamp(temperature, minTemperature, maxTemperature);

			if (adjustmentTimer.Tick())
			{
				AdjustGlow();
			}
		}
	}
}
