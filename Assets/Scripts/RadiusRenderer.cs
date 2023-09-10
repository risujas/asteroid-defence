using UnityEngine;

public class RadiusRenderer : MonoBehaviour
{
	[SerializeField] protected float radius = 1.0f;
	[SerializeField] protected int numVertices = 200;

	protected LineRenderer lineRenderer;

	protected virtual void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = false;
		lineRenderer.positionCount = numVertices + 1;

		Vector3[] positions = new Vector3[numVertices + 1];
		for (int i = 0; i <= numVertices; i++)
		{
			float angle = (float)i / numVertices * 360.0f;
			float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
			float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
			positions[i] = new Vector3(x, y, 0.0f);
		}

		lineRenderer.SetPositions(positions);
	}
}
