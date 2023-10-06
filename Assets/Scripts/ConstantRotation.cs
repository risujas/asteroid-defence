using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
	[SerializeField] protected Vector3 rotationSpeed;
	[SerializeField] protected bool runInFixedUpdate = true;

	private void Update()
	{
		if (!runInFixedUpdate)
		{
			transform.Rotate(rotationSpeed * Time.deltaTime);
		}
	}

	private void FixedUpdate()
	{
		if (runInFixedUpdate)
		{
			transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
		}
	}
}
