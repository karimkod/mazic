using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Exit : NetworkBehaviour {

	[SyncVar]
	public bool isOpened = false;
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){

		var collision = other.gameObject;

		if(collision.tag == "Runner"){
			Debug.Log ("A player just came in");
			var runner = collision.GetComponent<Runner> ();
			if(!isOpened && runner.HasKey){
				CmdOpenDoor ();
			}
			if(isOpened){
				runner.CmdWin ();
				//anim.SetTrigger ("OpenDoor");
			}
			//Destroy (this.gameObject);
		}

	}

	[Command]
	void CmdOpenDoor(){
		isOpened = true;

	}
}
