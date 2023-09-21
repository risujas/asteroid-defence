using UnityEngine;

public class AtmosphericDrag : MonoBehaviour
{
	[SerializeField] private float velocityDampenFactor;

	private void FixedUpdate()
	{
		for (int i = 0; i < Attractable.SpawnedAttractables.Count; i++)
		{
			var a = Attractable.SpawnedAttractables[i];
			float distance = Vector3.Distance(transform.position, a.transform.position);

			if (distance <= transform.localScale.x / 2.0f)
			{
				Rigidbody rb = a.GetComponent<Rigidbody>();

				var velocityReduction = rb.velocity * velocityDampenFactor * Time.deltaTime;
				rb.velocity -= velocityReduction;
			}
		}
	}
}
