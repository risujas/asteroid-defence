using UnityEngine;

public class OrbitalRadiusRenderer : RadiusRenderer
{
	[SerializeField] private GameObject parent;
	[SerializeField] private GameObject child;

	protected override void Start()
	{
		transform.position = parent.transform.position;
		radius = Vector3.Distance(parent.transform.position, child.transform.position);

		base.Start();
	}
}
