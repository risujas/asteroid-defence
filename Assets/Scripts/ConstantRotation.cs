using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
	[SerializeField] protected Vector3 rotationSpeed;

	public Vector3 RotationSpeed
	{
		set
		{
			rotationSpeed = value;
		}
		get
		{
			return rotationSpeed;
		}
	}

	private void Update()
	{
		transform.Rotate(rotationSpeed * Time.smoothDeltaTime);
	}
}
