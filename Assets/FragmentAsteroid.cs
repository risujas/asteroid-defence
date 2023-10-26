using UnityEngine;

public class FragmentAsteroid : MonoBehaviour
{
	[SerializeField] private float damage = 100000.0f;

	public void FragmentFromCollision(Collision collision)
	{
		if (collision.gameObject.GetComponent<Asteroid>() != null)
		{
			var health = collision.gameObject.GetComponent<Health>();
			health.ChangeHealth(-damage);
		}
	}
}
