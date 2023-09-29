using UnityEngine;
using UnityEngine.VFX;

public class Asteroid : GravityBody
{
	[SerializeField] private bool canSpawnFragments = false;
	[SerializeField] private GameObject impactEffectPrefab;

	private AsteroidSpawner asteroidSpawner;

	public void SpawnCollisionFragments()
	{
		if (!canSpawnFragments)
		{
			return;
		}

		Bounds bounds = GetComponent<Collider>().bounds;
		float totalFragmentableMass = rb.mass;

		while (totalFragmentableMass > 0.0f)
		{
			Vector3 spawnPoint = Vector3.zero;
			spawnPoint.x = Random.Range(bounds.min.x, bounds.max.x);
			spawnPoint.y = Random.Range(bounds.min.y, bounds.max.y);

			var newFragment = asteroidSpawner.SpawnFragment(spawnPoint);
			newFragment.rb.velocity = rb.velocity;

			totalFragmentableMass -= newFragment.rb.mass;
		}
	}

	private void SpawnCollisionEffects(Collision collision)
	{
		if (impactEffectPrefab != null)
		{
			bool collidedWithMajorBody = collision.gameObject.GetComponent<GravityBody>().IsMajorBody;
			Vector3 spawnPoint = collision.GetContact(0).point;

			var effect = Instantiate(impactEffectPrefab, spawnPoint, Quaternion.identity);
			effect.transform.up = rb.velocity.normalized;
			effect.transform.parent = spawnedObjectsContainer.transform;

			var vfx = effect.GetComponent<VisualEffect>();
			vfx.SetFloat("fragmentMaxVelocity", rb.velocity.magnitude * 1.5f);
			vfx.SetBool("useAltColor", collidedWithMajorBody);

			if (effect.TryGetComponent<FollowObject>(out var follower))
			{
				if (collidedWithMajorBody)
				{
					follower.objectToFollow = collision.gameObject;
					follower.offset = transform.position - collision.gameObject.transform.position;
				}
				else
				{
					follower.enabled = false;
				}
			}
		}
	}

	protected override void HandleCollision(Collision collision)
	{
		base.HandleCollision(collision);

		if (hasCollided)
		{
			SpawnCollisionEffects(collision);
		}
	}

	protected override void Start()
	{
		base.Start();

		asteroidSpawner = GameObject.FindWithTag("AsteroidSpawner").GetComponent<AsteroidSpawner>();
	}

	protected override void OnEnable()
	{
		base.OnEnable();

		SetRandomAngularVelocity(-45.0f, 45.0f);
	}
}
