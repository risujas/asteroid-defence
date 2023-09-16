using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour
{
	private static List<Attractable> spawnedAttractables = new();

	public static int RecommendedAttractablesLimit = 300;

	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();

	public static int NumAttractables => SpawnedAttractables.Count;

	public static bool IsAboveRecommendedAttractablesLimit => NumAttractables >= RecommendedAttractablesLimit;

	[SerializeField] private float mass;
	[SerializeField] private Vector3 velocity;

	private bool allowVelocityChange = true;
	private bool hasImpacted = false;
	private Vector3 impactPosition;

	private FragmentTrail fragmentTrail;

	public float Mass
	{
		get { return mass; }
		set { mass = value; }
	}

	public Vector3 Velocity => velocity;

	public void AddVelocity(Vector3 v)
	{
		if (!allowVelocityChange)
		{
			return;
		}

		velocity += v;
	}

	private void Start()
	{
		fragmentTrail = GetComponentInChildren<FragmentTrail>();
	}

	private void OnEnable()
	{
		spawnedAttractables.Add(this);
	}

	private void OnDisable()
	{
		spawnedAttractables.Remove(this);
	}

	private void Update()
	{
		transform.position += velocity * Time.smoothDeltaTime;

		if (hasImpacted)
		{
			float distance = Vector3.Distance(transform.position, impactPosition);
			if (distance >= transform.lossyScale.x)
			{
				Destroy(gameObject);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		hasImpacted = true;
		impactPosition = transform.position;
		allowVelocityChange = false;

		if (fragmentTrail != null)
		{
			fragmentTrail.DetachTrailFromParent();
		}

		GetComponent<Collider>().enabled = false;
	}
}