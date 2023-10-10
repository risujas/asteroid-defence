using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
	[SerializeField] private UiGroupFader uiGroupFader;
	[SerializeField] private FundsManager fundsManager;
	[SerializeField] private PlacementManager placementManager;

	public void TriggerDefeat()
	{
		uiGroupFader.SwitchToDefeatScreen();

		fundsManager.gameObject.SetActive(false);
		placementManager.gameObject.SetActive(false);
	}
}