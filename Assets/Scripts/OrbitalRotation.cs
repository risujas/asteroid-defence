using UnityEngine;

public class OrbitalRotation : ConstantRotation
{
	[SerializeField] private GameObject parentBody;
	[SerializeField] private GameObject childBody;
	[SerializeField] private bool clockwiseRotation;

	public static float GetOrbitalPeriod(float orbitalRadius, float G, float parentMass)
	{
		return 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(orbitalRadius, 3) / (G * parentMass));
	}

	private void Start()
	{
		float r = Vector3.Distance(parentBody.transform.position, childBody.transform.position);
		float M = parentBody.GetComponent<Attractor>().Mass;

		float orbitalPeriod = GetOrbitalPeriod(r, Attractor.G, M);
		float degreesPerSecond = 360.0f / orbitalPeriod;

		if (!clockwiseRotation)
		{
			degreesPerSecond *= -1;
		}

		rotationSpeed.y = degreesPerSecond;
	}
}
