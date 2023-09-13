using TMPro;
using UnityEngine;

public class TimescaleTextSetter : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private TimescaleChanger timescaleChanger;

	private void Update()
	{
		text.text = timescaleChanger.Level.ToString();
	}
}
