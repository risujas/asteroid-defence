using UnityEngine;

public class RingSystem : MonoBehaviour
{
	[SerializeField] private float innerRadius = 0.5f;
	[SerializeField] private float outerRadius = 1.0f;
	[SerializeField] private Color color;
	[SerializeField] private Color emission;
	[SerializeField] private float rotationSpeed = 0.0f;
	private Material material;

	private void SetMaterialAttributes()
	{
		if (material == null)
		{
			material = GetComponent<Renderer>().sharedMaterial;
		}

		material.SetFloat("_InnerRadius", innerRadius);
		material.SetFloat("_OuterRadius", outerRadius);
		material.SetColor("_Color", color);
		material.SetColor("_Emission", emission);
		material.SetFloat("_RotationSpeed", rotationSpeed);
	}

	private void OnValidate()
	{
		SetMaterialAttributes();
	}
}
