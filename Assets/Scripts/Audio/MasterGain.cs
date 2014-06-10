using UnityEngine;
using System.Collections;

public class MasterGain : MonoBehaviour 
{
	void Update () 
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKey(KeyCode.UpArrow))
				AudioAnalyzer.masterGain += .01f;
			else if (Input.GetKey(KeyCode.DownArrow))
				AudioAnalyzer.masterGain -= .01f;
			else if (Input.GetKeyDown(KeyCode.R))
				AudioAnalyzer.masterGain = 1f;
		}



		if (AudioAnalyzer.masterGain > 10f)
			AudioAnalyzer.masterGain = 10f;
		else if(AudioAnalyzer.masterGain < .2f)
			AudioAnalyzer.masterGain = .2f;
	}
}
