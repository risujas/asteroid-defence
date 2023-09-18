using UnityEngine;

public class RingSystem : MonoBehaviour
{
	[SerializeField] private RingSystem rearSideClone = null;
	[SerializeField] private float innerRadius = 0.5f;
	[SerializeField] private float outerRadius = 1.0f;
	[SerializeField] private Color color;
	[SerializeField] private Color emission;
	[SerializeField] private float smoothness = 0.5f;
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
		material.SetFloat("_Smoothness", smoothness);
		material.SetFloat("_RotationSpeed", rotationSpeed);

		if (rearSideClone != null)
		{
			if (rearSideClone.material == null)
			{
				rearSideClone.material = rearSideClone.GetComponent<Renderer>().sharedMaterial;
			}

			rearSideClone.innerRadius = innerRadius;
			rearSideClone.outerRadius = outerRadius;
			rearSideClone.color = color;
			rearSideClone.emission = emission;
			rearSideClone.smoothness = smoothness;
			rearSideClone.rotationSpeed = rotationSpeed;

			rearSideClone.material.SetFloat("_InnerRadius", innerRadius);
			rearSideClone.material.SetFloat("_OuterRadius", outerRadius);
			rearSideClone.material.SetColor("_Color", color);
			rearSideClone.material.SetColor("_Emission", emission);
			rearSideClone.material.SetFloat("_Smoothness", smoothness);
			rearSideClone.material.SetFloat("_RotationSpeed", rotationSpeed);
		}
	}

	private void OnValidate()
	{
		SetMaterialAttributes();
	}
}
