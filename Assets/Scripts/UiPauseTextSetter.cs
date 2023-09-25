using TMPro;
using UnityEngine;

public class UiPauseTextSetter : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private TimescaleChanger timescaleChanger;

	private void Update()
	{
		text.text = timescaleChanger.IsPaused ? "Resume" : "Pause";
	}
}
