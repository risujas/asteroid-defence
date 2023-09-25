using UnityEngine;

public class RandomColorTint : MonoBehaviour
{
	[SerializeField] private bool randomizeOnStart = true;
	[SerializeField] private string materialColorTintString = "_ColorTint";
	[SerializeField] private float minHue = 0.0f;
	[SerializeField] private float maxHue = 1.0f;
	[SerializeField] private float minSaturation = 0.0f;
	[SerializeField] private float maxSaturation = 1.0f;
	[SerializeField] private float minValue = 0.0f;
	[SerializeField] private float maxValue = 1.0f;
	[SerializeField, ReadOnly] private Color selectedColor = Color.white;

	private Material material = null;

	public void Randomize()
	{
		if (material == null)
		{
			material = GetComponent<Renderer>().material;
		}

		if (material.HasColor(materialColorTintString))
		{
			selectedColor = Random.ColorHSV(minHue, maxHue, minSaturation, maxSaturation, minValue, maxValue);
			material.SetColor(materialColorTintString, selectedColor);
		}
	}

	private void Start()
	{
		if (randomizeOnStart)
		{
			Randomize();
		}
	}
}
