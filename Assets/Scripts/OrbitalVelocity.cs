using UnityEngine;

public class OrbitalVelocity : MonoBehaviour
{
	[SerializeField] private Rigidbody parentBody;
	[SerializeField] private float oppositeOrbitalDistance = 1.0f;
	[SerializeField] private bool clockwiseRotation;

	public static float GetOrbitalPeriod(float orbitalRadius, float parentMass)
	{
		return 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(orbitalRadius, 3) / (GravityBody.G * parentMass));
	}

	public static float GetOrbitalVelocity(float orbitalRadius, float parentMass)
	{
		var orbitalPeriod = GetOrbitalPeriod(orbitalRadius, parentMass);
		var orbitalVelocity = 2 * Mathf.PI * orbitalRadius / orbitalPeriod;
		return orbitalVelocity;
	}

	public static float GetOrbitalVelocity(float apo, float per, float parentMass)
	{
		float mu = GravityBody.G * parentMass; // gravitational parameter of the central body
		float a = (apo + per) / 2; // length of the semi-major axis of the elliptical orbit
		float orbitalVelocity = Mathf.Sqrt(mu * (2 / apo - 1 / a)); // vis-viva equation
		return orbitalVelocity;
	}

	public void SetVelocityRelativeToParent()
	{
		var r = Vector3.Distance(parentBody.transform.position, transform.position);
		var v = GetOrbitalVelocity(r, oppositeOrbitalDistance, parentBody.mass);

		float angle = -90.0f;
		if (clockwiseRotation)
		{
			angle = 90.0f;
		}

		Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
		Vector3 parentChildDir = (parentBody.transform.position - transform.position).normalized;
		Vector3 result = (rot * parentChildDir).normalized;

		var rb = GetComponent<Rigidbody>();
		rb.velocity = result * v;
	}

	private void Start()
	{
		SetVelocityRelativeToParent();
	}
}
