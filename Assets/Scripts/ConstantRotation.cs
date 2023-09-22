using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
	[SerializeField] protected Vector3 rotationSpeed;

	private void Update()
	{
		transform.Rotate(rotationSpeed * Time.smoothDeltaTime);
	}
}
