using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Network : MonoBehaviour {

	NetworkManager nm;
	[Range(0,1)]
	public int chosenClass;
	public GameObject[] players;
	// Use this for initialization
	void Start () {
		nm = GetComponent<NetworkManager> ();
		if (chosenClass == 0)
			nm.playerPrefab = players [0];
		else
			nm.playerPrefab = players [1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
