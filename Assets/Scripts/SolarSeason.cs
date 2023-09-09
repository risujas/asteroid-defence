using UnityEngine;

public class SolarSeason : MonoBehaviour
{
	[SerializeField] private float yearLengthInSeconds = 720.0f;
	[SerializeField] private float axialTilt = 23.5f;

	private float yearElapsedSeconds = 0.0f;

	public float YearProgress => yearElapsedSeconds / yearLengthInSeconds;

	private void Update()
	{
		yearElapsedSeconds += Time.deltaTime;
		yearElapsedSeconds %= yearLengthInSeconds;

		float axisValue = 90.0f - axialTilt;
		transform.localRotation = Quaternion.Euler(360.0f * YearProgress, axisValue, 0.0f);
	}
}
