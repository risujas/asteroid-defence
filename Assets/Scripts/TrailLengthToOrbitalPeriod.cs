using UnityEngine;

[RequireComponent(typeof(OrbitalVelocity), typeof(TrailRenderer))]
public class TrailLengthToOrbitalPeriod : MonoBehaviour
{
	private void Start()
	{
		var ov = GetComponent<OrbitalVelocity>();
		var tr = GetComponent<TrailRenderer>();

		var dist = Vector3.Distance(ov.transform.position, ov.ParentBody.transform.position);
		float period = OrbitalVelocity.GetOrbitalPeriod(dist, ov.ParentBody.mass);

		tr.time = period;
	}
}
