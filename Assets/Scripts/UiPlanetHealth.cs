using TMPro;
using UnityEngine;

public class UiPlanetHealth : MonoBehaviour
{
	[SerializeField] private Health health = null;
	[SerializeField] private TextMeshProUGUI textObject = null;

	private void Start()
	{
		if (health == null)
		{
			health = GameObject.FindWithTag("CentralBody").GetComponent<Health>();
		}
	}

	private void Update()
	{
		textObject.text = Mathf.RoundToInt(health.HealthPercentage * 100.0f).ToString() + "%";
	}
}
