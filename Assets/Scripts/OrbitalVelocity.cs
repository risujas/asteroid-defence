using UnityEngine;

public class OrbitalVelocity : MonoBehaviour
{
	[SerializeField] private Rigidbody parentRb;
	[SerializeField] private bool clockwiseRotation;

	public static float GetOrbitalPeriod(float orbitalRadius, float parentMass)
	{
		return 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(orbitalRadius, 3) / (Attractor.G * parentMass));
	}

	public static float GetOrbitalVelocity(float orbitalRadius, float parentMass)
	{
		var orbitalPeriod = GetOrbitalPeriod(orbitalRadius, parentMass);
		var orbitalVelocity = 2 * Mathf.PI * orbitalRadius / orbitalPeriod;
		return orbitalVelocity;
	}

	private void Start()
	{
		var r = Vector3.Distance(parentRb.transform.position, transform.position);
		var v = GetOrbitalVelocity(r, parentRb.mass);

		float angle = -90.0f;
		if (clockwiseRotation)
		{
			angle = 90.0f;
		}

		Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
		Vector3 parentChildDir = (parentRb.transform.position - transform.position).normalized;
		Vector3 result = (rot * parentChildDir).normalized;

		var rb = GetComponent<Rigidbody>();
		rb.velocity = result * v;
	}
}
