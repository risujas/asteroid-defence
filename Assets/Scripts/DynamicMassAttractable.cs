using UnityEngine;

public class DynamicMassAttractable : Attractable
{
	[Header("Dynamic Mass")]
	[SerializeField] private bool calculateMassOnStart = false;
	[SerializeField] private float density;

	public void SetMassFromDensityAndScale()
	{
		float r = transform.localScale.x / 2.0f;
		float volume = (4.0f / 3.0f) * Mathf.PI * Mathf.Pow(r, 3);
		mass = volume * density;
	}

	protected override void Start()
	{
		base.Start();

		if (calculateMassOnStart)
		{
			SetMassFromDensityAndScale();
		}
	}
}