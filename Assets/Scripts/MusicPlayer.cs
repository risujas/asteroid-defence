using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
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
}
