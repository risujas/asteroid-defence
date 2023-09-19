using System;
using UnityEngine;

public class TimescaleChanger : MonoBehaviour
{
	private bool isPaused = false;
	private float[] levels = { 0.1f, 0.25f, 0.5f, 1.0f, 2.5f, 5.0f, 10.0f, 25.0f };
	private int index = 1;

	public float Level => levels[index];

	public bool IsPaused => isPaused;

	public void SetTimescale(float value)
	{
		if (!isPaused)
		{
			Time.timeScale = value;
		}
	}

	public void Increase()
	{
		ChangeLevel(1);
	}

	public void Decrease()
	{
		ChangeLevel(-1);
	}

	public void ChangeLevel(int i)
	{
		index += i;
		index = Math.Clamp(index, 0, levels.Length - 1);

		if (!isPaused)
		{
			Time.timeScale = levels[index];
		}
	}

	public void TogglePause()
	{
		if (!isPaused)
		{
			Time.timeScale = 0.0f;
			isPaused = true;
		}
		else
		{
			Time.timeScale = levels[index];
			isPaused = false;
		}
	}
}
