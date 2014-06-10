using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour 
{

	public int levelToLoad;

	#region Unity Methods

	void Update () 
	{
		if(Input.GetKey(KeyCode.LeftShift))
		{
			if(Input.GetKeyDown(KeyCode.L))
				Application.LoadLevel(levelToLoad);
		}
	}
	#endregion

	#region Actions
	#endregion	
}
