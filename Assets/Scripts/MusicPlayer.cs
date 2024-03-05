using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	private AudioSource audioSource;

	public void ToggleMusic()
	{
		if (audioSource.isPlaying)
		{
			audioSource.Stop();
		}
		else
		{
			audioSource.Play();
		}
	}

	private void Awake()
	{
		var oldPlayer = GameObject.FindGameObjectWithTag("MusicPlayer");
		if (oldPlayer != null && oldPlayer.gameObject != gameObject)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}
}
