using UnityEngine;

public class Fragmentable : MonoBehaviour
{
	protected const float minFragmentSpeedMultiplier = 0.25f;
	protected const float maxFragmentSpeedMultiplier = 0.75f;

	[SerializeField] private Fragment fragmentPrefab;

	private void SpawnCollisionFragments(Collision collision, Vector3 reflectionVector, Rigidbody rb)
	{
		if (fragmentPrefab == null)
		{
			return;
		}


		Rigidbody fragmentPfRb = fragmentPrefab.GetComponent<Rigidbody>();
		int numFragments = Mathf.RoundToInt(rb.mass / fragmentPfRb.mass) / 2;

		for (int i = 0; i < numFragments; i++)
		{
			if (Attractable.IsAboveRecommendedAttractablesLimit)
			{
				break;
			}

			if (rb.mass <= fragmentPfRb.mass)
			{
				break;
			}

			Vector3 individualVector = reflectionVector * Random.Range(minFragmentSpeedMultiplier, maxFragmentSpeedMultiplier);
			individualVector = Quaternion.AngleAxis(Random.Range(-30.0f, 30.0f), Vector3.forward) * individualVector;

			float width = transform.localScale.x * 0.5f;
			Vector3 spawnPoint = (collision.GetContact(0).point + collision.GetContact(0).normal * 0.05f) + (Random.insideUnitSphere.normalized * Random.Range(-width, width));
			spawnPoint.z = 0.0f;

			var newFragmentRb = Instantiate(fragmentPrefab, spawnPoint, Quaternion.identity, transform.parent).GetComponent<Rigidbody>();
			newFragmentRb.velocity = individualVector;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		Vector3 reflectionVector = Vector3.Reflect(rb.velocity, collision.GetContact(0).normal);
		SpawnCollisionFragments(collision, reflectionVector, rb);
	}
}
