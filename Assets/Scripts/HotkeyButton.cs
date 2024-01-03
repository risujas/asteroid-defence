using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HotkeyButton : MonoBehaviour
{
	[SerializeField] private KeyCode hotkey;
	[SerializeField] private TextMeshProUGUI hotkeyTMP;
	[SerializeField] private string hotkeyText;

	[Serializable] public class DeactivationEvent : UnityEvent { }
	[SerializeField] public DeactivationEvent OnDeactivation;

	private Button button;

	private void Start()
	{
		button = GetComponent<Button>();
		hotkeyTMP.text = "(" + hotkeyText + ")";
	}

	private void Update()
	{
		if (Input.GetKeyUp(hotkey))
		{
			button.onClick.Invoke();
		}
	}
}
