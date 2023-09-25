using UnityEngine;

public class FundsManager : MonoBehaviour
{
	[SerializeField] private float minFunds = 0.0f;
	[SerializeField] private float maxFunds = 1000.0f;
	[SerializeField] private float funds = 1000.0f;
	[SerializeField] private float fundsGainPerSecond = 5.0f;

	public float Funds { get { return funds; } set { funds = value; } }

	public void Update()
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
