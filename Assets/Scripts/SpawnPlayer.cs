using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnPlayer : NetworkBehaviour {

	NetworkUI nm;
	public GameObject chaser;
	public GameObject runner;
	// Use this for initialization
	void Awake () {
		nm = GameObject.Find ("NetworkManager").GetComponent<NetworkUI> ();

		if(nm.character == Character.Chaser){
		//	var chas = Instantiate (chaser);
		//	NetworkServer.Spawn (chas);
			DestroyImmediate(GameObject.Find("Runner"));
			nm.playerPrefab = GameObject.Find("Chaser");
		//	NetworkServer.Spawn (chaser);
			//chas.transform.parent = this.transform;
		}
		else{
		//	var run = Instantiate (runner);
		//	NetworkServer.Spawn (run);
			DestroyImmediate(GameObject.Find("Chaser"));
			nm.playerPrefab = GameObject.Find("Runner");
			//NetworkServer.Spawn (runner);
			//run.transform.parent = this.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
