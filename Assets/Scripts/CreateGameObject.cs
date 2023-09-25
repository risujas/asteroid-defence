using UnityEngine;
public class CreateGameObject : MonoBehaviour
{
	[SerializeField] private Transform origin;
	[SerializeField] private GameObject selectedObject;
	[SerializeField] private bool autoApply = false;
	[SerializeField] private float autoApplyDelay = 0.0f;

	private float initialTime = 0.0f;

	public void InstantiateSelected()
	{
		Instantiate(selectedObject, origin.position, origin.rotation);
	}

	private void Start()
	{
		initialTime = Time.time;
	}

	private void Update()
	{
		if (autoApply && Time.time >= initialTime + autoApplyDelay)
		{
			InstantiateSelected();
		}
	}
}