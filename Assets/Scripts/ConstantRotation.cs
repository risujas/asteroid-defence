using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
	[SerializeField] private Vector3 rotationSpeed;

	[Header("Randomization")]
	[SerializeField] private bool randomizeOnStart;
	[SerializeField] private Vector3 randomMin;
	[SerializeField] private Vector3 randomMax;

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

	private void Start()
	{
		if (randomizeOnStart)
		{
			rotationSpeed.x = Random.Range(randomMin.x, randomMax.x);
			rotationSpeed.y = Random.Range(randomMin.y, randomMax.y);
			rotationSpeed.z = Random.Range(randomMin.z, randomMax.z);
		}
	}

	private void Update()
	{
		transform.Rotate(rotationSpeed * Time.deltaTime);
	}
}
