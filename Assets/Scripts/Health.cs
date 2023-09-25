using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	[SerializeField] private float hitpoints = 100.0f;
	[SerializeField] private float minHitpoints = 0.0f;
	[SerializeField] private float maxHitpoints = 100.0f;
	[SerializeField] private bool enableRegeneration = false;
	[SerializeField] private float regenerationRate = 0.0f;

	private bool isDead = false;

	public bool IsDead => isDead;
	public float HitPoints => hitpoints;
	public float HealthPercentage => hitpoints / maxHitpoints;

	[Serializable] public class HealthEvent : UnityEvent { }
	[SerializeField] protected HealthEvent OnDeath;

	public void ChangeHealth(float change)
	{
		hitpoints += change;
	}

	private void Regenerate(float timeStep)
	{
		if (enableRegeneration)
		{
			hitpoints += regenerationRate * timeStep;
		}
	}

	private void ClampHealth()
	{
		hitpoints = Mathf.Clamp(hitpoints, minHitpoints, maxHitpoints);
	}

	private void HandleDeath()
	{
		if (!isDead)
		{
			if (hitpoints <= minHitpoints)
			{
				isDead = true;
				OnDeath.Invoke();
			}
		}
		else
		{
			if (hitpoints > minHitpoints)
			{
				isDead = false;
			}
		}
	}

	private void Update()
	{
		Regenerate(Time.deltaTime);
		ClampHealth();
		HandleDeath();
	}
}
