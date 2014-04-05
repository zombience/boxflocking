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
						if(f.trans.position.y <= lowerBound.position.y|| f.trans.position.y >= upperBound.position.y)
						{
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
						if(f.trans.position.z <= lowerBound.position.z|| f.trans.position.z >= upperBound.position.z)
						{
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

	public VHelper GetLocalFlockValues(FlockMember reference)
	{
		Vector3 avg = Vector3.zero;
		int count = 0;

		foreach(FlockMember f in pod)
		{
			if(f == reference) // Do not compare flockmember to itself
				continue;

			if(Vector3.Distance(reference.trans.position, f.trans.position) < FlockManager.settings.range)
			{
				avg += f.velocity;
				count ++;
			}
		}

		return new VHelper(avg, count);
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

	//TODO: create method that gets all flock values (e.g. average velocity, position, etc.) for each box once per frame, and flockmembers can access that information.
	//seems this would be much more efficient than each flockmember assessing the whole box each time
}



