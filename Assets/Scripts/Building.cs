using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour
{
	[SerializeField] private float buildCost = 20.0f;
	private bool finishedSinking = false;

	public float BuildCost => buildCost;

	public void StartSinking()
	{
		StartCoroutine(SinkBuildingIntoGround());
	}

	private IEnumerator SinkBuildingIntoGround()
	{
		float duration = 3.0f;

		float elapsed = 0.0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;

			// TODO

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
