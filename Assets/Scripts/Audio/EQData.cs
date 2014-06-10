using UnityEngine;
using System.Collections;


public class EQData : MonoBehaviour
{
	public static float[] spectrum;
	public static float[] min;
	public static float[] max;

	public float updateRangeSpeed;

	void Start()
	{
		spectrum = AudioAnalyzer.output;	
		
	}

	void Update()
	{

	}
}