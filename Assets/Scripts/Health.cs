using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Health : MonoBehaviour
{
	private static List<Health> healthObjects = new();

	public static IReadOnlyList<Health> HealthObjects => healthObjects.AsReadOnly();

	[SerializeField] private float hitpoints = 100.0f;

	[SerializeField] private VisualEffect damageVfx = null;
	[SerializeField] private VisualEffect deathVfx = null;

	public float Hitpoints => hitpoints;

	public void ChangeHealth(float value)
	{
		if (value < 0.0f)
		{
			if (damageVfx != null)
			{
				var vfx = Instantiate(damageVfx, transform.position, Quaternion.identity);
				vfx.transform.parent = transform;
			}
		}

		hitpoints += value;
		Debug.Log(name + " health changed by " + value);
	}

	private void OnEnable()
	{
		healthObjects.Add(this);
	}

	private void OnDisable()
	{
		healthObjects.Remove(this);
	}

	private void Update()
	{
		if (hitpoints <= 0.0f)
		{
			if (deathVfx != null)
			{
				Instantiate(deathVfx, transform.position, Quaternion.identity);
			}

			Destroy(gameObject);
		}
	}
}
