using UnityEngine;

public class Fragment : Attractable
{
	[SerializeField] private FragmentTrail fragmentTrail;

	protected override void Start()
	{
		base.Start();

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