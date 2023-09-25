using TMPro;
using UnityEngine;

public class UiFundsTextSetter : MonoBehaviour
{
	[SerializeField] private FundsManager fundsManager = null;
	[SerializeField] private TextMeshProUGUI textComponent = null;

	private void Start()
	{
		if (fundsManager == null)
		{
			fundsManager = GameObject.FindWithTag("FundsManager").GetComponent<FundsManager>();
		}
	}

	private void Update()
	{
		textComponent.text = Mathf.RoundToInt(fundsManager.Funds).ToString();
	}
}
