using UnityEngine;
using System.Collections;

public class Timer
{
	public float life;
	public float elapsed { get { return curTime; } }
	public float normalized { get { return curTime / life; } } // returns timer as a range between 0 and 1
	public float remaining { get { return life - elapsed; } }
	public bool isFinished { get { return elapsed >= life; } }
	public bool isPaused;

	protected float startTime;
	protected float pauseTime;
	protected float curTime { get { return (isPaused ? pauseTime : getTime) - startTime; } set { pauseTime = value; } }
	protected float getTime { get { return fixedTime ? Time.fixedTime : Time.time; } }
	protected bool fixedTime;

	public Timer(float _life, bool useFixedTime = false) { life = _life; fixedTime = useFixedTime; startTime = getTime; }

	public void Start() { startTime = (isPaused ? getTime - elapsed : getTime); isPaused = false; }

	/// <summary>
	/// toggles the timer between paused / unpaused
	/// </summary>
	public void PauseToggle()
	{
		if (!isPaused)
		{
			curTime = getTime;
			isPaused = true;
			return;
		}
		Start();
	}

	public void Pause()
	{
		if(!isPaused)
			curTime = getTime;
		isPaused = true;
	}
}
