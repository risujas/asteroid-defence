using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 45.0f;

	private void Update()
	{
		transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
	}
}
