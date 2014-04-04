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
	public float inRangeCount;
	
	public VHelper(Vector3 _vel, float _val)
	{
		vel = _vel;
		inRangeCount = _val;
	}
	
	public void AddVelVal(VHelper vh)
	{
		vel += vh.vel;
		inRangeCount += vh.inRangeCount;
	}
}


