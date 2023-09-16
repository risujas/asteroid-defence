using UnityEngine;

public class Fragmentable : MonoBehaviour
{
	protected const float minFragmentSpeedMultiplier = 0.25f;
	protected const float maxFragmentSpeedMultiplier = 0.75f;

	[SerializeField] private Attractable fragmentPrefab;

	private void SpawnCollisionFragments(Collision collision, Vector3 reflectionVector, Attractable attractable)
	{
		if (fragmentPrefab == null)
		{
			return;
		}

		int numFragments = Mathf.RoundToInt(attractable.Mass / fragmentPrefab.Mass);

		for (int i = 0; i < numFragments; i++)
		{
			if (Attractable.IsAboveRecommendedAttractablesLimit)
			{
				break;
			}

			attractable.Mass -= fragmentPrefab.Mass;

			Vector3 individualVector = reflectionVector * Random.Range(minFragmentSpeedMultiplier, maxFragmentSpeedMultiplier);
			individualVector = Quaternion.AngleAxis(Random.Range(-30.0f, 30.0f), Vector3.forward) * individualVector;

			float width = transform.localScale.x * 0.5f;
			Vector3 spawnPoint = (collision.GetContact(0).point + collision.GetContact(0).normal * 0.05f) + (Random.insideUnitSphere.normalized * Random.Range(-width, width));
			spawnPoint.z = 0.0f;

			var newFragment = Instantiate(fragmentPrefab, spawnPoint, Quaternion.identity, transform.parent).GetComponent<Attractable>();
			newFragment.AddVelocity(individualVector);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		Attractable attractable = GetComponent<Attractable>();
		Vector3 reflectionVector = Vector3.Reflect(attractable.Velocity, collision.GetContact(0).normal);
		SpawnCollisionFragments(collision, reflectionVector, attractable);
	}
}
