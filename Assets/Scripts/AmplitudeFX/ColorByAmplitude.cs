using UnityEngine;
using System.Collections;

public class ColorByAmplitude : MonoBehaviour 
{
	#region vars
	public Color high;
	public Color low;
	public int band;
	public float maxInput = 33f;

	protected Renderer rend;
	protected Material[] mats;
	#endregion
	
	#region Unity methods
	void Start () 
	{	
		rend = renderer;
		mats = rend.materials;
		band = Random.Range(0, 3);
		high.r = Random.Range(0f, 1f);
	}
	
	void Update () 
	{
		for(int i = 0; i < mats.Length; i++)
			mats[i].color = Color.Lerp(low, high, AudioAnalyzer.GetScaledOutput(band, maxInput, .1f, 1f));
		
		rend.materials = mats;
	}
	#endregion

	#region Actions
	#endregion
}
