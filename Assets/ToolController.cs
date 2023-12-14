using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{
	[SerializeField] private bool deactivateAllUponRightClick;
	[SerializeField] private List<HotkeyButton> controlledButtons = new();

	public void DeactivateControlledButtons()
	{
		foreach (var b in controlledButtons)
		{
			b.OnDeactivation.Invoke();
		}
	}

	private void Update()
	{
		if (deactivateAllUponRightClick)
		{
			if (Input.GetMouseButtonUp(1))
			{
				DeactivateControlledButtons();
			}
		}
	}
}
