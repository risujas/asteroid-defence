using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
	[SerializeField] private Vector3 rotationSpeed;

	private void Update()
	{
		transform.Rotate(rotationSpeed * Time.deltaTime);
	}
}
