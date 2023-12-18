using UnityEngine;
using UnityEngine.VFX;

public class MissileControl : MonoBehaviour
{
	[SerializeField] private float maxDeltaV = 10.0f;
	[SerializeField] private float remainingDeltaV = 10.0f;
	[SerializeField] private float acceleration = 1.0f;

	[SerializeField] private AudioSource rocketEngineSound;
	[SerializeField] private VisualEffect rocketEngineVfx;
	[SerializeField] private Light rocketEngineLight;

	private Rigidbody rb;
	private FundsManager fundsManager;

	private void HandleAcceleration()
	{
		if (remainingDeltaV > maxDeltaV)
		{
			remainingDeltaV = maxDeltaV;
		}

		if (remainingDeltaV > 0.0f)
		{
			float deltaV = acceleration * Time.deltaTime;
			if (remainingDeltaV < deltaV)
			{
				deltaV = remainingDeltaV;
			}

			remainingDeltaV -= deltaV;
			rb.AddForce(transform.up * deltaV, ForceMode.VelocityChange);

			if (!rocketEngineSound.isPlaying)
			{
				rocketEngineSound.Play();
				rocketEngineVfx.Play();
				rocketEngineLight.enabled = true;
			}
		}

		if (rocketEngineSound.isPlaying)
		{
			if (remainingDeltaV <= 0.0f)
			{
				rocketEngineSound.Stop();
				rocketEngineVfx.Stop();
				rocketEngineLight.enabled = false;
			}
		}
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		fundsManager = GameObject.FindWithTag("FundsManager").GetComponent<FundsManager>();
	}

	private void Update()
	{
		HandleAcceleration();
	}

	private void OnCollisionEnter(Collision collision)
	{
		fundsManager.AddFundsFromAsteroidDestruction(collision);
	}
}
