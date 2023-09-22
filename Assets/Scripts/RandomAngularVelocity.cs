using UnityEngine;

public class RandomAngularVelocity : MonoBehaviour
{
	[SerializeField] private Rigidbody rb;
	[SerializeField] private Vector3 randomMin;
	[SerializeField] private Vector3 randomMax;

	private void Start()
	{
		Vector3 rot = Vector3.zero;
		rot.x = Random.Range(randomMin.x, randomMax.x) * Mathf.Deg2Rad;
		rot.y = Random.Range(randomMin.y, randomMax.y) * Mathf.Deg2Rad;
		rot.z = Random.Range(randomMin.z, randomMax.z) * Mathf.Deg2Rad;
		rb.angularVelocity = rot;
	}
}
