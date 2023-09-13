using TMPro;
using UnityEngine;

public class PauseTextSetter : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private TimescaleChanger timescaleChanger;

	private void Update()
	{
		text.text = timescaleChanger.IsPaused ? "Resume" : "Pause";
	}
}
