using UnityEngine;
using System.Collections;

[System.Serializable]
public class FlockSettings
{
	public float range; // "visible" range of neighbors
	public float maxVelocity; 
	public float acceleration;
	public float coherence; // how much weight to give to neighbors' average velocity
	public float individuality;
	public float seekCenter; // how much flockmember aims for center of the flock
	public float noise; 
	public bool asteroidsX;
	public bool asteroidsY;
	public bool asteroidsZ;

}
