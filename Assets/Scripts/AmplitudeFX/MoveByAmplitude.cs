using UnityEngine;
using System.Collections;

public class MoveByAmplitude : MonoBehaviour 
{

	public bool local = true;
	[Range(0, 3)]
	public int listenBand;
	public float amount;
	public Vector3 direction;

	public bool usePhysics;

	protected Vector3 origin;
	protected Transform trans;
	protected Rigidbody body;

	#region Unity Methods
	void Start ()
	{
		trans = transform;
		origin = local ? trans.localPosition : trans.position;
		if (usePhysics)
			body = rigidbody;
	}
	
	void Update () 
	{
		if (usePhysics)
		{
			//body.AddForce(trans.forward * EQData.spectrum[listenBand] * amount, ForceMode.Impulse);
			body.velocity = trans.forward * AudioAnalyzer.output[listenBand] * amount;
			return;
		}
		if(local)
			trans.localPosition = origin + (direction * AudioAnalyzer.output[listenBand] * amount);
		else
			trans.position = origin + (direction * AudioAnalyzer.output[listenBand] * amount);
	}
	#endregion
}
