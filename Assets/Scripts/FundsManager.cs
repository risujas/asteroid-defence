using UnityEngine;

public class FundsManager : MonoBehaviour
{
	[SerializeField] private float minFunds = 0.0f;
	[SerializeField] private float maxFunds = 1000.0f;
	[SerializeField] private float funds = 1000.0f;
	[SerializeField] private float fundsGainPerSecond = 5.0f;
	[SerializeField] private float fundsImpactModifier = 10.0f;
	[SerializeField] private float fundsDestructionModifier = 20.0f;

	public float Funds { get { return funds; } set { funds = value; } }

	public void AddFundsFromAsteroidDestruction(Collision collision)
	{
		var asteroid = collision.gameObject.GetComponent<Asteroid>();
		if (asteroid == null)
		{
			return;
		}

		float value = collision.rigidbody.mass * fundsDestructionModifier;
		Funds += value;
	}

	public void AddFundsFromAsteroidImpact(Collision collision)
	{
		var asteroid = collision.gameObject.GetComponent<Asteroid>();
		if (asteroid == null)
		{
			return;
		}

		float value = collision.rigidbody.mass * fundsImpactModifier;
		Funds += value;
	}

	private void Update()
	{
		if (funds < minFunds)
		{
			funds = minFunds;
		}

		funds += fundsGainPerSecond * Time.deltaTime;

		if (funds > maxFunds)
		{
			funds = maxFunds;
		}
	}
}
