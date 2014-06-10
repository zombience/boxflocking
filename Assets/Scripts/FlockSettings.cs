using UnityEngine;
using System.Collections;

[System.Serializable]
public class FlockSettings
{
	public bool softBoundaries;
	public float softBoundaryForce;
	public float range; // "visible" range of neighbors
	public float maxVelocity; 
	public float acceleration;
	public float coherence; // how much weight to give to neighbors' average velocity

	public float seekCenterMass; // how much flockmember aims for center of the flock
	public float noise;
	public float audioNoiseMod;
	public float noiseAccumulateSmoothing;
	public float repulsion; // how much weight to give to keeping distance from neighbors
	public float closenessCutoff; // if neighbors are closer than this, try to move away

	public float seekCenter; // how much flockmember aims for center of the flock

	public bool asteroidsX;
	public bool asteroidsY;
	public bool asteroidsZ;


}
