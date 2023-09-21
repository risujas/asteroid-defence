using UnityEngine;

public class Fragment : Attractable
{
	[SerializeField] private FragmentTrail fragmentTrail;

	private void Start()
	{
		if (fragmentTrail == null)
		{
			fragmentTrail = GetComponentInChildren<FragmentTrail>();
		}
	}

	protected override void HandleCollision()
	{
		base.HandleCollision();

		if (fragmentTrail != null)
		{
			fragmentTrail.DetachTrailFromParent();
		}
	}
}