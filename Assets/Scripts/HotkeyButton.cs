using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HotkeyButton : MonoBehaviour
{
	[SerializeField] private KeyCode hotkey;
	[SerializeField] private TextMeshProUGUI hotkeyText;

	[Serializable] public class DeactivationEvent : UnityEvent { }
	[SerializeField] public DeactivationEvent OnDeactivation;

	private Button button;

	private void Start()
	{
		button = GetComponent<Button>();
		hotkeyText.text = "(" + hotkey.HumanName() + ")";
	}

	private void Update()
	{
		if (Input.GetKeyUp(hotkey))
		{
			button.onClick.Invoke();
		}
	}
}
