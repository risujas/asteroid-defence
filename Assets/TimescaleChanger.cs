using TMPro;
using UnityEngine;

public class TimescaleChanger : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textField;

	private float[] levels = { 0.1f, 1.0f, 3.0f, 10.0f, 50.0f, 100.0f };
	private int index = 0;

	public void Increase()
	{
		index++;
		if (index >= levels.Length)
		{
			index--;
		}
		SetTimescale();
	}

	public void Decrease()
	{
		index--;
		if (index < 0)
		{
			index = 0;
		}
		SetTimescale();
	}

	private void SetTimescale()
	{
		Time.timeScale = levels[index];
		textField.text = "x" + levels[index].ToString();
	}
}
