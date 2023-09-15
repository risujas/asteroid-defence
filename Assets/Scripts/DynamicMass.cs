using UnityEngine;

public class DynamicMass : MonoBehaviour
{
	[SerializeField] private bool calculateMassOnStart = false;
	[SerializeField] private float density;

	private Attractable attractable = null;

	public void SetMassFromDensityAndScale()
	{
		if (attractable == null)
		{
			attractable = GetComponent<Attractable>();
		}

		float r = transform.localScale.x / 2.0f;
		float volume = (4.0f / 3.0f) * Mathf.PI * Mathf.Pow(r, 3);
		attractable.Mass = volume * density;
	}

	private void Start()
	{
		if (calculateMassOnStart)
		{
			SetMassFromDensityAndScale();
		}
	}
}