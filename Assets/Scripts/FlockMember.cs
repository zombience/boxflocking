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

	#region Unity Methods
	void Start ()
	{
		velocity = Random.insideUnitSphere;
		trans = transform;
	}
	#endregion

	#region Actions
	public void Move(FlockBox box)
	{
		curValues =  box.GetLocalFlockValues(this);

		velocity = Vector3.Lerp(velocity, 
		                        Vector3.ClampMagnitude(FlockManager.settings.noise*Random.onUnitSphere*FlockManager.settings.maxVelocity+(velocity * FlockManager.settings.individuality) + (AverageVelocity(box) * FlockManager.settings.coherence), 
		                       FlockManager.settings.maxVelocity), Time.fixedDeltaTime * FlockManager.settings.acceleration);
		velocity *= FlockManager.settings.maxVelocity / Vector3.Magnitude (velocity);
		trans.position += velocity * speed;
	}

	public bool CompareBoxes()
	{
		return newBox.i != oldBox.i || newBox.j != oldBox.j || newBox.k != oldBox.k;
	}

	protected Vector3 AverageVelocity(FlockBox box)
	{
		VHelper avg = new VHelper(Vector3.zero, 0);

		for(int i = 0; i < box.neighbors.Length; i++)
		{
			avg.AddVelVal(FlockManager.instance.boxes[box.neighbors[i].i, box.neighbors[i].j, box.neighbors[i].k].GetLocalFlockValues(this));
		}

		/*
		if(avg.val == 0)
			return Vector3.zero;
		else
			return avg.vec / avg.val;
			*/
		return avg.vel.normalized;
	}




	#endregion	
}
