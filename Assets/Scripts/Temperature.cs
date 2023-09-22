using UnityEngine;

public class Temperature : MonoBehaviour
{
	[SerializeField] private Material material = null;
	[SerializeField] private string materialEmissionString = "_EmissionIntensity";
	[SerializeField] private float maxEmissionIntensity = 10.0f;
	[SerializeField] private float temperature = 0.0f;

	private float temperatureDecay = 10.0f;
	private float minTemperature = 0.0f;
	private float maxTemperature = 1000.0f;

	public float MaxTemperature => maxTemperature;

	public void ChangeTemperature(float change)
	{
		temperature += change;
	}

	private void AdjustGlow()
	{
		if (material.HasFloat(materialEmissionString))
		{
			float t = temperature / maxTemperature * maxEmissionIntensity;
			material.SetFloat(materialEmissionString, t);
		}
		else
		{
			Debug.Log("Shader material: " + material + " on object:" + gameObject.name + " doesn't contain the emission control float: " + materialEmissionString);
			enabled = false;
		}
	}

	private void Start()
	{
		if (material == null)
		{
			material = GetComponent<Renderer>().material;
		}
	}

	private void FixedUpdate()
	{
		if (temperature > 0.0f)
		{
			temperature -= temperatureDecay * Time.deltaTime;
			temperature = Mathf.Clamp(temperature, minTemperature, maxTemperature);

			AdjustGlow();
		}
	}
}
