using UnityEngine;

public class Fragmentable : MonoBehaviour
{
	protected const float minFragmentSpeedMultiplier = 0.25f;
	protected const float maxFragmentSpeedMultiplier = 0.75f;

	[SerializeField] private Attractable fragmentPrefab;

	public void SpawnCollisionFragments(Collision collision, Vector3 reflectionVector, float mass)
	{
		if (fragmentPrefab != null)
		{
			int numFragments = Mathf.RoundToInt(mass / fragmentPrefab.Mass);

			for (int i = 0; i < numFragments; i++)
			{
				Vector3 individualVector = reflectionVector * Random.Range(minFragmentSpeedMultiplier, maxFragmentSpeedMultiplier);
				individualVector = Quaternion.AngleAxis(Random.Range(-30.0f, 30.0f), Vector3.forward) * individualVector;

				float width = transform.localScale.x * 0.5f;
				Vector3 spawnPoint = (collision.GetContact(0).point + collision.GetContact(0).normal * 0.05f) + (Random.insideUnitSphere.normalized * Random.Range(-width, width));
				spawnPoint.z = 0.0f;

				var newFragment = Instantiate(fragmentPrefab, spawnPoint, Quaternion.identity, transform.parent).GetComponent<Attractable>();
				newFragment.AddVelocity(individualVector);
			}
		}
	}
}
