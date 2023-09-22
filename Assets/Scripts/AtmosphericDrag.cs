using System.Collections.Generic;
using UnityEngine;

public class AtmosphericDrag : MonoBehaviour
{
	[SerializeField] private float velocityReductionFactor = 0.5f;
	[SerializeField] private List<Rigidbody> ignoredRigidbodies = new();

	private void OnTriggerStay(Collider other)
	{
		if (ignoredRigidbodies.Contains(other.attachedRigidbody))
		{
			return;
		}

		var velocityReduction = other.attachedRigidbody.velocity * velocityReductionFactor * Time.deltaTime;
		other.attachedRigidbody.velocity -= velocityReduction;

		var temperature = other.GetComponent<Temperature>();
		if (temperature != null)
		{
			temperature.ChangeTemperature(temperature.MaxTemperature * velocityReductionFactor * Time.deltaTime);
		}
	}
}
