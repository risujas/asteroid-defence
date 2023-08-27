using UnityEngine;

public class FragmentOnImpact : MonoBehaviour
{
	[SerializeField] private GameObject fragmentPrefab;

	private void SpawnFragments(int numFragments, Vector3 forces)
	{
		for (int i = 0; i < numFragments; i++)
		{
			var newFragment = Instantiate(fragmentPrefab, transform.position, Quaternion.identity);
			var rb = newFragment.GetComponent<Rigidbody>();
			rb.velocity = forces;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (fragmentPrefab == null)
		{
			return;
		}

		var thisRb = GetComponent<Rigidbody>();
		var otherRb = collision.collider.GetComponent<Rigidbody>();

		if (thisRb.mass > otherRb.mass)
		{
			return;
		}

		Vector3 direction = (collision.transform.position - transform.position).normalized;
		Vector3 surfaceNormal = collision.GetContact(0).normal;

		Vector3 reflectionVector = Vector3.Reflect(direction, surfaceNormal);
		reflectionVector = reflectionVector.normalized * 0.75f;

		SpawnFragments(100, reflectionVector);
		Destroy(gameObject);
	}
}
