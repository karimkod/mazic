using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : MonoBehaviour {
	Transform target;
	float x;
	float y;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

			
		if(target != null)
			transform.position = new Vector3(target.position.x,target.position.y,transform.position.z);
	}

	public void setFollowed(Transform followed){
		target = followed;
	}
}
