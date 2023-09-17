using UnityEngine;

public class PhysicsSync : MonoBehaviour
{
	private void Update()
	{
		Physics.SyncTransforms();
	}
}
