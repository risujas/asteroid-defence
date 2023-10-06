using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
	[SerializeField] private UiGroupFader uiGroupFader;

	public void TriggerDefeat()
	{
		uiGroupFader.SwitchToDefeatScreen();
	}
}
