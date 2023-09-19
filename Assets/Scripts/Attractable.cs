using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour
{
	protected static List<Attractable> spawnedAttractables = new();

	public static int RecommendedAttractablesLimit = 300;
	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();
	public static int NumAttractables => SpawnedAttractables.Count;
	public static bool IsAboveRecommendedAttractablesLimit => NumAttractables >= RecommendedAttractablesLimit;

	[SerializeField] protected float mass;
	[SerializeField] protected Vector3 velocity;
	[SerializeField] protected LayerMask collisionLayerMask;
	[SerializeField] protected bool useRaycastCollision = false;

	protected bool allowVelocityChange = true;
	protected bool hasImpacted = false;
	protected Vector3 impactPosition;

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

	protected virtual void HandleCollision()
	{
		hasImpacted = true;
		impactPosition = transform.position;
		allowVelocityChange = false;

		GetComponent<Collider>().enabled = false;
	}

	protected virtual void Start()
	{
	}

	protected void OnEnable()
	{
		spawnedAttractables.Add(this);
	}

	protected void OnDisable()
	{
		spawnedAttractables.Remove(this);
	}

	protected void Update()
	{
		Vector3 deltaPosition = velocity * Time.deltaTime;
		Vector3 nextPosition = transform.position + deltaPosition;

		if (useRaycastCollision)
		{
			Ray ray = new Ray(transform.position, deltaPosition.normalized);
			if (Physics.Raycast(ray, out RaycastHit hit, deltaPosition.magnitude, collisionLayerMask))
			{
				nextPosition = hit.point;
			}
		}

		transform.position = nextPosition;

		if (hasImpacted)
		{
			float distance = Vector3.Distance(transform.position, impactPosition);
			if (distance >= transform.lossyScale.x)
			{
				Destroy(gameObject);
			}
		}
	}

	protected void OnCollisionEnter(Collision collision)
	{
		HandleCollision();
	}
}