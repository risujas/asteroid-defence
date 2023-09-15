using UnityEngine;

public class OrbitalRotation : MonoBehaviour
{
	[SerializeField] private Attractor parentBody;
	[SerializeField] private bool clockwiseRotation;
	[SerializeField] private Vector3 rotation = Vector3.zero;
	[SerializeField, ReadOnly] private float degreesPerSecond;

	public static float GetOrbitalPeriod(float orbitalRadius, float G, float parentMass)
	{
		return 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(orbitalRadius, 3) / (G * parentMass));
	}

	private void Start()
	{
		float r = Vector3.Distance(parentBody.transform.position, transform.position);
		float M = parentBody.GetComponent<Attractor>().Mass;

		float orbitalPeriod = GetOrbitalPeriod(r, Attractor.G, M);
		degreesPerSecond = 360.0f / orbitalPeriod;

		if (!clockwiseRotation)
		{
			degreesPerSecond *= -1;
		}
	}

	private void Update()
	{
		float r = Vector3.Distance(parentBody.transform.position, transform.position);

		float deg = degreesPerSecond;
		if (clockwiseRotation)
		{
			deg *= -1;
		}

		rotation += new Vector3(0.0f, 0.0f, deg) * Time.smoothDeltaTime;
		rotation.x %= 360.0f;
		rotation.y %= 360.0f;
		rotation.z %= 360.0f;

		Quaternion rot = Quaternion.Euler(rotation);
		Vector3 dir = rot * Vector3.down;

		transform.position = parentBody.transform.position + (r * dir);
	}
}
