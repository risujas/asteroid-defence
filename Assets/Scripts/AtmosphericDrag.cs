using System.Collections.Generic;
using UnityEngine;

public class AtmosphericDrag : MonoBehaviour
{
	[SerializeField] private float velocityReductionFactor = 0.5f;
	[SerializeField] private float sizeReductionFactor = 0.15f;
	[SerializeField] private List<Rigidbody> ignoredRigidbodies = new();

	private void OnTriggerStay(Collider other)
	{
		if (ignoredRigidbodies.Contains(other.attachedRigidbody))
		{
			return;
		}

		other.attachedRigidbody.velocity -= other.attachedRigidbody.velocity * velocityReductionFactor * Time.deltaTime;

		float scaleModifier = Mathf.Lerp(1.0f, 0.0f, sizeReductionFactor * Time.deltaTime);
		other.transform.localScale *= scaleModifier;
		other.attachedRigidbody.mass *= Mathf.Pow(scaleModifier, 3);

		var temperature = other.GetComponent<Temperature>();
		if (temperature != null)
		{
			temperature.ChangeTemperature(temperature.MaxTemperature * velocityReductionFactor * Time.deltaTime);
		}
	}
}
