using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
	[SerializeField] protected Vector3 rotationSpeed;

	private void FixedUpdate()
	{
		transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
	}
}
