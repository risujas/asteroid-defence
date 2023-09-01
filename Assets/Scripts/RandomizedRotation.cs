using UnityEngine;

public class RandomizedRotation : ConstantRotation
{
	[Header("Randomization")]
	[SerializeField] private bool randomizeOnStart;
	[SerializeField] private Vector3 randomMin;
	[SerializeField] private Vector3 randomMax;

	private void Start()
	{
		if (randomizeOnStart)
		{
			rotationSpeed.x = Random.Range(randomMin.x, randomMax.x);
			rotationSpeed.y = Random.Range(randomMin.y, randomMax.y);
			rotationSpeed.z = Random.Range(randomMin.z, randomMax.z);
		}
	}
}
