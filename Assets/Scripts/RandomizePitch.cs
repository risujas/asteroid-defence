using UnityEngine;

public class RandomizePitch : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource = null;
	[SerializeField] private float minimumPitch = 0.9f;
	[SerializeField] private float maximumPitch = 1.1f;

	private void Start()
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}

		audioSource.pitch = Random.Range(minimumPitch, maximumPitch);
	}
}
