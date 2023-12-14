using UnityEngine;

public class LaserCannonControl : MonoBehaviour
{
	private void Update()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;
		transform.up = (mousePos - transform.position).normalized;
	}
}
