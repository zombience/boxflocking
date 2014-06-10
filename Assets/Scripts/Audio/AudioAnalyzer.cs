using UnityEngine;
using System.Collections;
 
[RequireComponent(typeof(AudioSource))]
public class AudioAnalyzer : MonoBehaviour 
{

	public const int BANDS = 4;
	public static float[] output = new float[BANDS]; // averaged amplitude of freqData per band
	public static float[] freqData = new float[sampleCount]; // raw output data of every analyzed sample: values between -1.0 to 1.0
	public static float masterGain = 1f;

	public float mGain;

	public int[] crossovers = new int[BANDS]; // split the spectrum into bands at each crossover point 
	public float[] curve = new float[BANDS]; // multiply the amplitude of each band. higher frequencies require more boosting

	public bool easeAmplitude; // should amplitudes be eased from previous values, or directly assigned new raw averages?
	[Range(0.001f, .1f)]
	public float climbRate;
	[Range(0.001f, .1f)]
	public float fallRate;
	
	public bool passThrough;

	protected SelectInputGUI micSelector;
	protected AudioSource source;
	protected float[] band = new float[BANDS]; // used for accumulating freqData
	protected const int sampleCount = 512; // larger sample sizes will yield more "accurate" analysis at the cost of slower analysis
	protected string selectedDevice;
	protected int minFreq, maxFreq;
	
	//TODO: put this in another thread, see if it improves FPS on mac
	
    void Start() 
	{
		
		source = audio;

		source.loop = true; // Set the AudioClip to loop
		source.mute = true; // Mute the sound, we don't want the player to hear it

		selectedDevice = Microphone.devices[0].ToString();
		Debug.Log("selected device: " + selectedDevice);

		Microphone.GetDeviceCaps(selectedDevice, out minFreq, out maxFreq);//Gets the frequency of the device
		if ((minFreq + maxFreq) == 0)
			maxFreq = 48000;

		StartCoroutine(ManageBuffer());
    }

	void Update()
	{
		mGain = masterGain;

		GetMultibandAmplitude();

		if (Input.GetKeyDown(KeyCode.I))
		{
			if (micSelector == null)
			{
				micSelector = gameObject.AddComponent<SelectInputGUI>();
				micSelector.SetCallback(SetInputDevice);
			}
			else
			{
				Destroy(micSelector);
				micSelector = null;
			}
		}

		if (source.mute == passThrough)
			source.mute = !passThrough;	 // this is for debugging, making sure the audio in is usable
	}

	public static float GetScaledOutput(int listenBand, float bandMax,float targetMin, float targetMax)
	{
		return output[listenBand].Map(0, bandMax, targetMin, targetMax);
	}

	public void SetInputDevice(string device)
	{
		StopMicrophone();
		selectedDevice = device;
	}

	protected void StopMicrophone () 
	{
		source.Stop();//Stops the source
		Microphone.End(selectedDevice);//Stops the recording of the device	
	}

	protected IEnumerator ManageBuffer()
	{
		while (true)
		{
			Timer bufferTimer = new Timer(5f);
			while (!bufferTimer.isFinished)
				yield return bufferTimer;

			// stop playing audio and halt mic recording
			source.Stop(); 
			Microphone.End(selectedDevice);
				
			// set new clip to new recording and wait for recording to being before playing source
			source.clip = Microphone.Start(selectedDevice, true, 10, maxFreq);
			while (Microphone.GetPosition(selectedDevice) <= 0)
				yield return Microphone.GetPosition(selectedDevice); 

			source.Play();
		}
	}
		
	/// <summary>
	/// GetAveragedVolume returns the average volume of the entire signal
	/// </summary>
	/// <returns></returns>
	protected float GetAverageAmplitude() 
	{
        float[] data = new float[sampleCount];

        float a = 0;
		audio.GetSpectrumData(data, 0, FFTWindow.Hamming);

        foreach(float s in data) 
            a += Mathf.Abs(s);
        
        return a/sampleCount;
    }

	protected void GetMultibandAmplitude()
	{
		source.GetSpectrumData(freqData, 0, FFTWindow.Hamming);

		int k = 0;
		float[] lengths = new float[BANDS];
		for(int i = 0; i < BANDS; i++)
		{
			float min = (i > 0 ? crossovers[i-1] : 0); 
			lengths[i] = crossovers[i] - min; 
			band[i] = 0f;
		}

		for (int i = 0; i < freqData.Length; i++)
		{
			

			if (k > BANDS - 1)
				break;

			band[k] += freqData[i];

			if (i > crossovers[k])
			{
				if(easeAmplitude)
				{
					float bandAmp = Mathf.Abs(band[k] / lengths[k]) * curve[k];
					output[k] = Mathf.Lerp(output[k], bandAmp * masterGain, bandAmp * (bandAmp > output[k] ? climbRate : fallRate)); // if analyzed amplitude is larger than previous amplitude, ease to new amplitude at climbRate, otherwise ease at fallRate
				}
				else
					output[k] = Mathf.Abs(band[k] / lengths[k]) * curve[k] * masterGain; 

				k++;
			}
		}
	}
}