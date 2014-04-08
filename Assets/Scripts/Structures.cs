using UnityEngine;
using System.Collections;

/// <summary>
/// IJK is intended to be used to keep track of flockbox locations and register flockmembers in the appropriate lists
/// </summary>
public struct IJK
{
	public int i;
	public int j;
	public int k;
	public IJK(int _i, int _j, int _k)
	{
		i = _i;
		j = _j;
		k = _k;
	}
}


/// <summary>
/// Vector helper
/// VHelper is intended to be a struct that can be used to pack values returned from flockmemebers in various boxes. 
/// Additional values and methods can be added to contain whichever values must be returned from the flock
/// Examples may include location, speed, rotation, etc. 
/// </summary>
public struct VHelper
{
	public Vector3 vel;
	public Vector3 center; // use to average center of mass
	public float inRangeCount; // float so that division can be done accurately without cast
	public FlockMember closest;

	public VHelper(Vector3 _vel, float _val)
	{
		this = new VHelper(_vel, Vector3.zero, _val);
	}

	public VHelper(Vector3 _vel, Vector3 c, float _val)
	{
		this = new VHelper(_vel, Vector3.zero, _val, null);
	}

	public VHelper(Vector3 _vel, Vector3 c, float _val, FlockMember f)
	{
		vel = _vel;
		center = c;
		inRangeCount = _val;
		closest = f;
	}
	
	public void AddValues(VHelper vh)
	{
		vel += vh.vel;
		center += vh.center;
		inRangeCount += vh.inRangeCount;
	}

	public void SetClosest(FlockMember f)
	{
		closest = f;
	}


}


