using System.Collections;
using UnityEngine;

public class PlaceableBuilding : Placeable
{
	private bool finishedSinking = false;

	public void StartSinking()
	{
		var parentAnchor = GetComponentInParent<PlacementAnchor>();

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
