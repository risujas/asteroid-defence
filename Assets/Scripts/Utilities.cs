using UnityEngine;

static class Utilities
{
	public static void DrawAxes(Transform transform, float length, float duration)
	{
		Debug.DrawLine(transform.position, transform.position + transform.forward * length, Color.blue, duration);
		Debug.DrawLine(transform.position, transform.position + -transform.forward * length, Color.grey, duration);

		Debug.DrawLine(transform.position, transform.position + transform.up * length, Color.green, duration);
		Debug.DrawLine(transform.position, transform.position + -transform.up * length, Color.grey, duration);

		Debug.DrawLine(transform.position, transform.position + transform.right * length, Color.red, duration);
		Debug.DrawLine(transform.position, transform.position + -transform.right * length, Color.grey, duration);
	}
}