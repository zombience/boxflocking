using UnityEngine;
using System.Collections;

public class SelectInputGUI : MonoBehaviour 
{

	protected Callback<string> callback;

	void OnGUI()
	{
		float spacing = 220;
		Rect r = new Rect(100, Screen.height - 100, 200, 50);
		for (int i = 0; i < Microphone.devices.Length; i++)
		{
			if (GUI.Button(r, Microphone.devices[i]))
				callback(Microphone.devices[i]);
			r = new Rect(r.xMin + spacing, r.yMin, r.width, r.height);
		}
	}


	public void SetCallback(Callback<string> cb)
	{
		callback = cb;
	}

	
}
