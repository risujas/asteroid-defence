using TMPro;
using UnityEngine;

public class UiTimescaleTextSetter : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI timescaleLevelText;
	[SerializeField] private TextMeshProUGUI actualTimescaleText;
	[SerializeField] private TimescaleChanger timescaleChanger;

	private void Update()
	{
		timescaleLevelText.text = timescaleChanger.Level.ToString();

		if (timescaleChanger.Level != Time.timeScale)
		{
			actualTimescaleText.text = "(" + Time.timeScale.ToString() + ")";
		}
		else
		{
			actualTimescaleText.text = "";
		}
	}
}
