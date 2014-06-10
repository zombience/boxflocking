using UnityEngine;
using System.Collections;

public class RotateByAmplitude : MonoBehaviour 
{
	#region vars
	public Vector3 axis;
	public float speed;
	[Range(0,3)]
	public int listenBand;
	public bool randomize;
	public bool useThreshold;
	public float threshold;

	public float randomShiftTime;
	protected Transform trans;
	protected Timer timer;

	protected Quaternion target;
	protected float val;
	#endregion
	
	#region Unity methods
	void Start () 
	{	
		trans = transform;
		if(randomize)
			StartCoroutine(RandomizeAxis());
		if (useThreshold)
			StartCoroutine(ThresholdCheck());
	}
	
	void Update () 
	{

		val = AudioAnalyzer.output[listenBand];
		if (useThreshold)
			trans.rotation = Quaternion.Lerp(trans.rotation, target, Time.fixedDeltaTime * speed);
		else
			trans.Rotate(axis, speed * val);
	}
	#endregion
	
	#region Actions

	protected IEnumerator ThresholdCheck()
	{
		while (true)
		{
			if (val > threshold)
			{
				Timer t = new Timer(.2f);
				target = Random.rotation;
				while (!t.isFinished)
					yield return t;
				
			}
			yield return null;
		}
	}

	protected IEnumerator RandomizeAxis()
	{
		timer = new Timer(randomShiftTime);
		Vector3 targ = Random.insideUnitSphere;

		while(!timer.isFinished)
		{
			axis = Vector3.Lerp(axis, targ, timer.normalized);
			yield return null;
		}
		StartCoroutine(RandomizeAxis());
	}
	#endregion
}
