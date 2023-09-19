using UnityEngine;
using UnityEngine.VFX;

public class ImpactEffect : MonoBehaviour
{
	private const float ejectionVfxSpeedMultiplier = 0.35f;

	[SerializeField] private GameObject impactEffectPrefab;

	private GameObject spawnedObjectsContainer;

	private void SpawnCollisionEffects(Collision collision, Vector3 reflectionVector)
	{
		if (impactEffectPrefab != null)
		{
			Vector3 spawnPoint = collision.GetContact(0).point;

			var effect = Instantiate(impactEffectPrefab, spawnPoint, Quaternion.identity);
			effect.transform.up = reflectionVector;
			effect.transform.parent = spawnedObjectsContainer.transform;

			var vfx = effect.GetComponent<VisualEffect>();
			if (vfx != null)
			{
				if (vfx.HasFloat("ejectionSpeed"))
				{
					vfx.SetFloat("ejectionSpeed", reflectionVector.magnitude * ejectionVfxSpeedMultiplier);
				}
			}

			var follower = effect.GetComponent<FollowObject>();
			if (follower != null)
			{
				follower.objectToFollow = collision.gameObject;
				follower.offset = transform.position - collision.gameObject.transform.position;
			}
		}
	}

	private void Start()
	{
		spawnedObjectsContainer = GameObject.FindWithTag("SpawnedObjectsContainer");
	}

	private void OnCollisionEnter(Collision collision)
	{
		Attractable attractable = GetComponent<Attractable>();
		Vector3 reflectionVector = Vector3.Reflect(attractable.Velocity, collision.GetContact(0).normal);
		SpawnCollisionEffects(collision, reflectionVector);
	}
}