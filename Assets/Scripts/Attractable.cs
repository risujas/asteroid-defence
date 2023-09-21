using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attractable : MonoBehaviour
{
	protected static List<Attractable> spawnedAttractables = new();

	public static int RecommendedAttractablesLimit = 300;
	public static IReadOnlyList<Attractable> SpawnedAttractables => spawnedAttractables.AsReadOnly();
	public static int NumAttractables => SpawnedAttractables.Count;
	public static bool IsAboveRecommendedAttractablesLimit => NumAttractables >= RecommendedAttractablesLimit;

	protected bool allowVelocityChange = true;
	protected bool hasImpacted = false;
	protected Vector3 impactPosition;

	[Serializable] public class AttractableEvent : UnityEvent { }
	public AttractableEvent OnImpact;

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

	protected void FixedUpdate()
	{
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
		OnImpact.Invoke();
	}
}