using UnityEngine;

public class MissileControl : MonoBehaviour
{
	[SerializeField] private float maxDeltaV = 10.0f;
	[SerializeField] private float remainingDeltaV = 10.0f;
	[SerializeField] private float acceleration = 1.0f;

	private Rigidbody rb;
	private CameraControl cameraControl;
	private FundsManager fundsManager;
	private TimescaleChanger timescaleChanger;
	private AudioSource rocketEngineSound;

	private void HandleRotation()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;
		Vector3 direction = mousePos - transform.position;

		transform.up = direction.normalized;
	}

	private void HandleAcceleration()
	{
		if (remainingDeltaV > maxDeltaV)
		{
			remainingDeltaV = maxDeltaV;
		}

		if (Input.GetMouseButton(0))
		{
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
				}
			}
		}

		if (rocketEngineSound.isPlaying)
		{
			if (Input.GetMouseButtonUp(0) || remainingDeltaV <= 0.0f)
			{
				rocketEngineSound.Stop();
			}
		}
	}

	private void OnEnable()
	{
		timescaleChanger = GameObject.FindWithTag("TimescaleChanger").GetComponent<TimescaleChanger>();
		timescaleChanger.SetTimescale(1.0f);
	}
	private void OnDisable()
	{
		timescaleChanger.ResetTimescale();
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		cameraControl = Camera.main.GetComponent<CameraControl>();
		cameraControl.FollowedObject = gameObject;

		fundsManager = GameObject.FindWithTag("FundsManager").GetComponent<FundsManager>();

		rocketEngineSound = GetComponent<AudioSource>();
	}

	private void Update()
	{
		HandleRotation();
		HandleAcceleration();

		if (Input.GetMouseButtonUp(1))
		{
			if (cameraControl.FollowedObject == gameObject)
			{
				cameraControl.FollowedObject = null;
			}

			enabled = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		fundsManager.AddFundsFromAsteroidDestruction(collision);
	}
}
