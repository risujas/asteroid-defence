using UnityEngine;

public class RingSystem : MonoBehaviour
{
	[SerializeField] private Material defaultMaterial;
	[SerializeField] private GameObject rearSide;

	[SerializeField] private float innerRadius = 0.5f;
	[SerializeField] private float outerRadius = 1.0f;
	[SerializeField] private Color color = Color.white;
	[SerializeField] private float smoothness = 0.66f;
	[SerializeField] private float rotationSpeed = 0.0f;
	[SerializeField] private float noiseScale = 7000.0f;
	[SerializeField] private float noiseStrength = 0.3f;

	private Material material = null;

	private void SetMaterialAttributes()
	{
		material = Instantiate(defaultMaterial);
		GetComponent<Renderer>().sharedMaterial = material;
		rearSide.GetComponent<Renderer>().sharedMaterial = material;

		material.SetFloat("_InnerRadius", innerRadius);
		material.SetFloat("_OuterRadius", outerRadius);
		material.SetColor("_Color", color);
		material.SetFloat("_Smoothness", smoothness);
		material.SetFloat("_RotationSpeed", rotationSpeed);
		material.SetFloat("_NoiseScale", noiseScale);
		material.SetFloat("_NoiseStrength", noiseStrength);
	}

	private void Start()
	{
		SetMaterialAttributes();
	}

	private void OnValidate()
	{
		SetMaterialAttributes();
	}
}
