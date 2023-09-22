using UnityEngine;

public class IntervalTimer
{
	protected float interval;
	protected float last;
	private bool init = false;

	public float Interval
	{
		get { return interval; }
		set { interval = value; }
	}

	public IntervalTimer(float interval)
	{
		this.interval = interval;
		last = Mathf.NegativeInfinity;
	}

	public virtual bool Tick()
	{
		if (!init)
		{
			init = true;
			last = Time.time;
		}

		if (Time.time >= last + interval)
		{
			last = Time.time;
			return true;
		}

		return false;
	}
}

public class IntervalChanceTimer : IntervalTimer
{
	protected float chance;

	public float Chance
	{
		get { return chance; }
		set { chance = Mathf.Clamp01(value); }
	}

	public IntervalChanceTimer(float interval, float chance) : base(interval)
	{
		this.chance = chance;
	}

	public override bool Tick()
	{
		if (base.Tick())
		{
			float roll = Random.Range(0.0f, 1.0f);
			if (roll < chance)
			{
				return true;
			}
		}

		return false;
	}
}