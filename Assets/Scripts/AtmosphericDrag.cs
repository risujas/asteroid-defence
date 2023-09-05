using UnityEngine;

public class AtmosphericDrag : MonoBehaviour
{
	[SerializeField] private float velocityDampenFactor;

	private void Update()
	{
		for (int i = 0; i < Attractable.SpawnedAttractables.Count; i++)
		{
			var a = Attractable.SpawnedAttractables[i];
			float distance = Vector3.Distance(transform.position, a.transform.position);

			if (distance <= transform.localScale.x / 2.0f)
			{
				var velocityReduction = a.Velocity * velocityDampenFactor * Time.deltaTime;
				a.AddVelocity(-velocityReduction);
			}
		}
	}
}
