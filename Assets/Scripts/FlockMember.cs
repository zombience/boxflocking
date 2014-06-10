using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockMember : MonoBehaviour 
{
	//public Vector3 position;
	public Vector3 velocity;

	public IJK newBox;
	public IJK oldBox;
	public float speed;
	public bool hasMoved;

	public Transform trans;

	public List<FlockMember> neighborhood = new List<FlockMember>();
	protected VHelper curValues;
	protected Renderer rend;
	protected int band;
	protected Vector3 noiseDirection;
	protected Vector3 noiseAccumulate;
	#region Unity Methods
	void Start ()
	{
		velocity = Random.insideUnitSphere;
		trans = transform;
		rend = renderer;
		band = Random.Range(0, AudioAnalyzer.BANDS);
		if(band == 0 || band == 1)
			noiseDirection = Vector3.forward;
		else if(band == 2)
			noiseDirection = Vector3.right;
		else 
			noiseDirection = Vector3.up;

	}
	#endregion

	#region Actions
	public void Move(FlockBox box)
	{
		curValues =  box.GetLocalFlockValues(this);
		rend.material.color = Color.grey + (curValues.inRangeCount / (FlockManager.instance.flockSize * .1f)) * Color.red; // The more flock members in range, the brighter the flockmember
		velocity = Vector3.Lerp(
								velocity, 
		                        Vector3.ClampMagnitude(CalculateVelocity(box), FlockManager.settings.maxVelocity),
		                        Time.fixedDeltaTime * FlockManager.settings.acceleration
		                        );

		velocity *= FlockManager.settings.maxVelocity / Vector3.Magnitude (velocity);
		trans.position += velocity * speed;

		trans.forward = velocity.normalized;
	}

	public bool CompareBoxes()
	{
		return newBox.i != oldBox.i || newBox.j != oldBox.j || newBox.k != oldBox.k;
	}

	protected VHelper GetNeighborFlockData(FlockBox box)
	{
		VHelper avg = new VHelper(Vector3.zero, 0);

		for(int i = 0; i < box.neighbors.Length; i++)
		{
			avg.AddValues(FlockManager.instance.boxes[box.neighbors[i].i, box.neighbors[i].j, box.neighbors[i].k].GetLocalFlockValues(this, avg.closest));
		}

		return avg;
	}

	/// <summary>
	/// Calculates the velocity
	/// This method puts in once place all the various calculations resulting in the final simplified velocity
	/// This is to visually simplify editing for future tweaking
	/// </summary>
	/// <returns>The velocity.</returns>
	protected Vector3 CalculateVelocity(FlockBox box)
	{
		VHelper values = GetNeighborFlockData(box);
		Vector3 v = Vector3.zero;
		/* Noise */
		noiseAccumulate = Vector3.Lerp(noiseAccumulate, new Vector3(AudioAnalyzer.output[0], AudioAnalyzer.output[1], AudioAnalyzer.output[2]), Time.fixedDeltaTime * FlockManager.settings.noiseAccumulateSmoothing);
		v += Random.onUnitSphere * (FlockManager.settings.noise + Vector3.Dot(noiseDirection, noiseAccumulate));
		/* Basic Direction Follow */
		v += values.vel.normalized * FlockManager.settings.coherence;
		/* cluster towards center of current grouping */
		if(values.inRangeCount > 0)
			v += ((values.center / values.inRangeCount) - trans.position).normalized  * FlockManager.settings.seekCenterMass; // stick to the center of flock

		/* maintain separation */
		//TODO: get separation working
		/*
		if(values.closest != null)
			v += (values.closest.trans.position - trans.position) * FlockManager.settings.separation;
		*/
		return v;
	}




	#endregion	
}
