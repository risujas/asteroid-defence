using System;
using UnityEngine;

public class TimescaleChanger : MonoBehaviour
{
	[SerializeField] private bool isPaused = false;
	[SerializeField] private float[] levels = { 0.1f, 0.25f, 0.5f, 1.0f, 2.5f, 5.0f, 10.0f, 25.0f };
	[SerializeField] private int levelIndex = 1;
	[SerializeField, ReadOnly] private float activeTimeScale;

	public float Level => levels[levelIndex];
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
		levelIndex += i;
		levelIndex = Math.Clamp(levelIndex, 0, levels.Length - 1);

		if (!isPaused)
		{
			Time.timeScale = levels[levelIndex];
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
			Time.timeScale = levels[levelIndex];
			isPaused = false;
		}
	}

	private void Update()
	{
		activeTimeScale = Time.timeScale;
	}

	private void OnValidate()
	{
		if (isPaused)
		{
			Time.timeScale = 0.0f;
		}
		else
		{
			Time.timeScale = levels[levelIndex];
		}
	}
}
