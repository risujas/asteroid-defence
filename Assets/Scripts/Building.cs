using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
	[SerializeField] private float buildCost = 20.0f;
	[SerializeField] private bool allowMultiPlacement = true;
	[SerializeField] private GameObject placementEffect = null;
	[SerializeField] private bool addToParentHierarchy = true;

	private bool finishedSinking = false;

	public float BuildCost => buildCost;

	public bool AllowMultiPlacement => allowMultiPlacement;

	public GameObject PlacementEffect => placementEffect;

	public bool AddToParentHierarchy => addToParentHierarchy;

	[Serializable] public class PlacementEvent : UnityEvent { }

	public PlacementEvent placementEvent;

	public void StartSinking()
	{
		var parentAnchor = GetComponentInParent<BuildingAnchor>();

		StartCoroutine(SinkBuildingIntoGround(parentAnchor.SpawnHeight));
	}

	private IEnumerator SinkBuildingIntoGround(float requiredDistance)
	{
		Vector3 initialPosition = transform.position;
		float sinkingSpeed = 0.03f;
		bool finished = false;

		while (!finished)
		{
			transform.position += -transform.up * sinkingSpeed * Time.deltaTime;

			float distance = Vector3.Distance(initialPosition, transform.position);
			if (distance >= requiredDistance)
			{
				finished = true;
			}

			yield return null;
		}

		finishedSinking = true;
		yield return null;
	}

	private void Update()
	{
		if (finishedSinking)
		{
			Destroy(gameObject);
		}
	}
}
