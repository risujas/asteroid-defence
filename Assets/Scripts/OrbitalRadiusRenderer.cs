using UnityEngine;

public class OrbitalRadiusRenderer : MonoBehaviour
{
	[SerializeField] private GameObject parent;
	[SerializeField] private GameObject child;
	[SerializeField] private int numVertices = 200;

	private LineRenderer lineRenderer;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = false;
		lineRenderer.positionCount = numVertices + 1;

		float radius = Vector3.Distance(parent.transform.position, child.transform.position);

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
