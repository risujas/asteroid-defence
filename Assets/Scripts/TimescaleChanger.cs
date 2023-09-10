using TMPro;
using UnityEngine;

public class TimescaleChanger : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timescaleText;
	[SerializeField] private TextMeshProUGUI pauseButtonText;

	private bool paused = false;
	private float[] levels = { 0.1f, 1.0f, 3.0f, 10.0f, 50.0f, 100.0f };
	private int index = 1;

	public float Level => levels[index];

	public void Increase()
	{
		index++;
		if (index >= levels.Length)
		{
			index--;
		}

		if (!paused)
		{
			Time.timeScale = levels[index];
		}
		timescaleText.text = "x" + levels[index].ToString();
	}

	public void Decrease()
	{
		index--;
		if (index < 0)
		{
			index = 0;
		}

		if (!paused)
		{
			Time.timeScale = levels[index];
		}
		timescaleText.text = "x" + levels[index].ToString();
	}

	public void HandlePauseButton()
	{
		if (!paused)
		{
			Time.timeScale = 0.0f;
			pauseButtonText.text = "Resume";
			paused = true;
		}
		else
		{
			Time.timeScale = levels[index];
			timescaleText.text = "x" + levels[index].ToString();

			pauseButtonText.text = "Pause";
			paused = false;
		}
	}
}
