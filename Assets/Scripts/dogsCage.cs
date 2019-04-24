using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class dogsCage : NetworkBehaviour {

	// Use this for initialization

	public GameObject[] dogs;
	bool isReleased = false;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){

		var collision = other.gameObject;

		if(collision.tag == "Chaser"){
			if(!isReleased){
				CmdReleaseDogs ();
				isReleased = true;
			}
		}

	}

	[Command]
	public void CmdReleaseDogs(){
		
		foreach(GameObject dog in dogs){
			GameObject d = Instantiate (dog, this.transform);
		}
	}

}
