using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FlockManager : MonoBehaviour 
{

	public int boxCountX,boxCountY,boxCountZ;
	public FlockBox[,,] boxes;
	public Transform lowerBound;
	public Transform upperBound;
	public FlockMember flockPrefab;
	public static FlockSettings settings;
	public FlockSettings flockSettings;

	public int flockSize;

	public static FlockManager instance;

	protected Vector3 center;

	#region Unity Methods
	void Start ()
	{
		instance = this;
		settings = flockSettings;
		flockSettings.range = (upperBound.position.x - lowerBound.position.x) / boxCountX;
		boxes = new FlockBox[boxCountX, boxCountY, boxCountZ];

		for(int i = 0; i < boxCountX; i ++)
		{
			for(int j = 0; j < boxCountY; j ++)
			{
				for(int k = 0; k < boxCountZ; k ++)
				{
					if(FlockManager.settings.softBoundaries && (i == 0 || i == boxCountX - 1 || j == 0 || j == boxCountY - 1 || k == 0 || k == boxCountZ -1))
						boxes[i,j,k] = new SoftBoundaryBox(lowerBound, upperBound);
					else
						boxes[i,j,k] = new FlockBox();

					boxes[i,j,k].SetNeighborhood(i,j,k,boxCountX,boxCountY,boxCountZ);
				}
			}
		}

		GenerateFlock();
	}
	
	void Update () 
	{
		MoveFlock();
	}
	#endregion

	#region Actions
	protected void GenerateFlock()
	{
		for(int i = 0; i < flockSize; i++)
		{
			Vector3 pos = Random.insideUnitSphere;

			pos.x *= (upperBound.position.x - lowerBound.position.x)/2;
			pos.y *= (upperBound.position.y - lowerBound.position.y)/2;
			pos.z *= (upperBound.position.z - lowerBound.position.z)/2;

			pos += (upperBound.position - lowerBound.position)/ 2 + lowerBound.position;

			FlockMember f = (FlockMember)Instantiate(flockPrefab, pos, Quaternion.identity);
			f.transform.forward = Random.onUnitSphere;
			f.transform.parent = transform; // This is purely for visual organization in the heirarchy window
			FillBox(f);
		}
	}

	protected void FillBox(FlockMember f)
	{
		f.newBox = f.oldBox = Checkbox(f);
		boxes[f.newBox.i,f.newBox.j,f.newBox.k].pod.Add(f);
	}

	protected void MoveFlock()
	{
		List<FlockMember> removable = new List<FlockMember>();



		for(int i = 0; i < boxCountX; i ++)
		{
			for(int j = 0; j < boxCountY; j ++)
			{
				for(int k = 0; k < boxCountZ; k ++)
				{
					foreach(FlockMember f in boxes[i,j,k].pod)
					{
						f.Move(boxes[i,j,k]);

						 
						if(f.trans.position.x <= lowerBound.position.x || f.trans.position.x >= upperBound.position.x)
						{

							if(FlockManager.settings.asteroidsX){
								if(f.trans.position.x <= lowerBound.position.x){
									Vector3 pos = f.trans.position;
									pos.x = (pos.x-lowerBound.position.x)+upperBound.position.x;
									f.trans.position = pos;
									         } else {
									Vector3 pos = f.trans.position;
									pos.x = (pos.x-upperBound.position.x)+lowerBound.position.x;
									f.trans.position = pos;
									}
							} else{
								f.velocity = Vector3.Reflect(f.velocity, Vector3.right);
								if(f.trans.position.x <= lowerBound.position.x) 
								{
									Vector3 pos = f.trans.position;
									pos.x = (lowerBound.position.x - f.trans.position.x) + lowerBound.position.x;
									f.trans.position = pos;
								}
								
								else
								{
									Vector3 pos = f.trans.position;
									pos.x = (upperBound.position.x - f.trans.position.x) + upperBound.position.x;
									f.trans.position = pos;
								}
							}
						}
				

						
						if(f.trans.position.y <= lowerBound.position.y|| f.trans.position.y >= upperBound.position.y)
						{
							
							if(FlockManager.settings.asteroidsY){
								if(f.trans.position.y <= lowerBound.position.y){
									Vector3 pos = f.trans.position;
									pos.y = (pos.y-lowerBound.position.y)+upperBound.position.y;
									f.trans.position = pos;
								} else {
									Vector3 pos = f.trans.position;
									pos.y = (pos.y-upperBound.position.y)+lowerBound.position.y;
									f.trans.position = pos;
								}
							} else{
								f.velocity = Vector3.Reflect(f.velocity, Vector3.up);
								if(f.trans.position.y <= lowerBound.position.y) 
								{
									Vector3 pos = f.trans.position;
									pos.y = (lowerBound.position.y - f.trans.position.y) + lowerBound.position.y;
									f.trans.position = pos;
								}
								
								else
								{
									Vector3 pos = f.trans.position;
									pos.y = (upperBound.position.y - f.trans.position.y) + upperBound.position.y;
									f.trans.position = pos;
								}
							}
						}


						if(f.trans.position.z <= lowerBound.position.z|| f.trans.position.z >= upperBound.position.z)
						{

							if(FlockManager.settings.asteroidsZ){
								if(f.trans.position.z <= lowerBound.position.z){
									Vector3 pos = f.trans.position;
									pos.z = (pos.z-lowerBound.position.z)+upperBound.position.z;
									f.trans.position = pos;
								} else {
									Vector3 pos = f.trans.position;
									pos.z = (pos.z-upperBound.position.z)+lowerBound.position.z;
									f.trans.position = pos;
								}
							} else{
								f.velocity = Vector3.Reflect(f.velocity, Vector3.forward);
								if(f.trans.position.z <= lowerBound.position.z) 
								{
									Vector3 pos = f.trans.position;
									pos.z = (lowerBound.position.z - f.trans.position.z) + lowerBound.position.z;
									f.trans.position = pos;
								}
								
								else
								{
									Vector3 pos = f.trans.position;
									pos.z = (upperBound.position.z - f.trans.position.z) + upperBound.position.z;
									f.trans.position = pos;
								}
								
							}
						}

						f.oldBox = new IJK(f.newBox.i, f.newBox.j, f.newBox.k);
						f.newBox = Checkbox(f);

						if(f.CompareBoxes())
							removable.Add(f);
					}
				}
			}
		}
		Rebox(removable);
	}

	public FlockMember GetRandomFlockMember()
	{

		IJK rand = new IJK(Random.Range(0, boxCountX - 1), Random.Range(0, boxCountY -1), Random.Range(0, boxCountZ -1));
		Debug.Log ("searching for box member: " + rand.i + " " + rand.j + " " + rand.k + " vs. " + boxCountX + "," + boxCountY + "," + boxCountZ);
		while(boxes[rand.i, rand.j, rand.k].pod.Count < 1)
		{
			rand = new IJK(Random.Range(0, boxCountX -1), Random.Range(0, boxCountY -1), Random.Range(0, boxCountZ -1));
			Debug.Log("searching for non null flockmember");
		}

		return boxes[rand.i, rand.j, rand.k].pod[0];
	}

	protected IJK Checkbox(FlockMember f)
	{
		float x = f.transform.position.x - lowerBound.position.x;
		float width = upperBound.position.x - lowerBound.position.x;
		float y = f.transform.position.y - lowerBound.position.y;
		float height = upperBound.position.y - lowerBound.position.y;
		float depth = upperBound.position.z - lowerBound.position.z;
		float z = f.transform.position.z - lowerBound.position.z;
		return new IJK(Mathf.FloorToInt(x / width * boxCountX), Mathf.FloorToInt(y / height * boxCountY), Mathf.FloorToInt(z / depth * boxCountZ));
	}

	protected void Rebox(List<FlockMember> removable)
	{
		// TODO: cheap fix of ArrayIndexOutOfRange with modulo: solve actual problem instead 
		foreach(FlockMember f in removable)
		{
			if(boxes[f.oldBox.i % boxCountX,f.oldBox.j % boxCountY,f.oldBox.k % boxCountZ].pod.Contains(f))  
			{
				boxes[f.oldBox.i % boxCountX,f.oldBox.j % boxCountY,f.oldBox.k % boxCountZ].pod.Remove(f);
				boxes[f.newBox.i % boxCountX,f.newBox.j % boxCountY,f.newBox.k % boxCountZ].pod.Add(f); 
			}
		}
	}
	#endregion	
}


public class FlockBox
{
	public IJK[] neighbors;
	public List<FlockMember> pod = new List<FlockMember>();

	public FlockBox(){}

	public virtual VHelper GetLocalFlockValues(FlockMember reference, FlockMember prevClosest = null)
	{
		Vector3 avgVel = Vector3.zero;
		Vector3 avgPos = Vector3.zero;
		int count = 0;
		float curDistance = FlockManager.settings.closenessCutoff;
		FlockMember closest = prevClosest; // prevClosest may be null
		foreach(FlockMember f in pod)
		{
			if(f == reference || Vector3.Distance(reference.trans.position, f.trans.position) > FlockManager.settings.range) // Do not compare flockmember to itself or if other is out of range
				continue;

			avgVel += f.velocity;
			avgPos = f.trans.position;
			count ++;
			float dist = Vector3.Distance(reference.trans.position, f.trans.position);
			if(dist < curDistance)
			{
				closest = f;
				curDistance = dist;	
			}
		}
		// if we have a previous closest and it is closer than the current box's closest, pass it to the next iteration to compare
		if(prevClosest != null && Vector3.Distance(prevClosest.trans.position, reference.trans.position) < curDistance)
			closest = prevClosest; 

		return new VHelper(avgVel, avgPos, count, closest);
	}

	public void SetNeighborhood(int i, int j, int k, int boxCountX, int boxCountY, int boxCountZ)
	{
		int[] iList = CreateArray(i, boxCountX);
		int[] jList = CreateArray(j, boxCountY);
		int[] kList = CreateArray(k, boxCountZ);

		neighbors = new IJK[((iList.Length * jList.Length * kList.Length))];

		int nIndex = 0;
		for(int ii = 0; ii < iList.Length; ii++)
		{
			for(int jj = 0; jj < jList.Length; jj++)
			{
				for(int kk = 0; kk < kList.Length; kk++)
				{
					if(iList[ii] == i && jList[jj] == j && kList[kk] == k) // don't assign self to neighborhood
						continue;
					neighbors[nIndex] = new IJK(iList[ii], jList[jj] , kList[kk]);
					nIndex ++;
				}	
			}
		}
	}

	protected int[] CreateArray(int val, int boxCount)
	{
		if(boxCount == 1) {
				int[] arr = {val};
				return arr;
		} else {
				if (val > 0 && val < boxCount - 1) {
						int[] arr = {val - 1, val, val + 1};
						return arr;
				} else {
						int[] arr = {val == 0 ? val : val - 1, val == 0 ? val + 1 : val};
						return arr;
				}
			}
	}
}
//TODO: get soft boundary boxes working
public class SoftBoundaryBox : FlockBox
{
	public Transform lowerBoundary;
	public Transform upperBoundary;

	public SoftBoundaryBox(){}
	public SoftBoundaryBox(Transform l, Transform u)
	{
		lowerBoundary = l;
		upperBoundary = u;
	}

	public override VHelper GetLocalFlockValues (FlockMember reference, FlockMember prevClosest)
	{
		VHelper vals = base.GetLocalFlockValues (reference, prevClosest);
		vals.vel = ((lowerBoundary.position + upperBoundary.position) / 2) * FlockManager.settings.softBoundaryForce; // aim velocity towards the center of the box for a "soft" boundary effect
		return vals;
	}
}

