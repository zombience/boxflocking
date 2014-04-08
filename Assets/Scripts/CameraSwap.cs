using UnityEngine;
using System.Collections;

public class CameraSwap : MonoBehaviour 
{

	public bool follow;
	protected bool flip;

	public float followSpeed;

	public Transform followCam;
	public Transform staticCam;


	protected FlockMember guide;

	#region Unity Methods
	void Update () 
	{
		if(follow != flip)
		{
			followCam.gameObject.SetActive(follow);
			staticCam.camera.enabled = !follow;
			guide = FlockManager.instance.GetRandomFlockMember();
			flip = follow;
		}

		if(follow)
		{
			followCam.position = Vector3.Lerp(followCam.position, guide.trans.position, Time.fixedDeltaTime * followSpeed);
			followCam.forward = Vector3.Lerp(followCam.forward, guide.trans.forward, Time.fixedDeltaTime * followSpeed);
		}
	}
	#endregion
}
