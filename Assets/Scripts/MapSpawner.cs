using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class MapSpawner : NetworkBehaviour {

	public GameObject maze;
	// Use this for initialization
	public override void OnStartServer()
	{
			Vector3 spawnPosition = new Vector3(0.0f,0.0f,0.0f);
			GameObject _maze = Instantiate(maze, spawnPosition,transform.rotation);
			NetworkServer.Spawn(_maze);

	}
}
