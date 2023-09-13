using UnityEngine;

public class FollowObject : MonoBehaviour
{
	public GameObject objectToFollow;
	public Vector3 offset = Vector3.zero;

	private void Update()
	{
		transform.position = objectToFollow.transform.position + offset;
	}
}
