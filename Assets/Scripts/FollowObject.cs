using UnityEngine;

public class FollowObject : MonoBehaviour
{
	public GameObject objectToFollow = null;
	public Vector3 offset = Vector3.zero;

	private void Update()
	{
		if (objectToFollow == null)
		{
			enabled = false;
			return;
		}

		transform.position = objectToFollow.transform.position + offset;
	}
}
