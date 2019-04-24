using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	
	public float speed = 3.5f;
	[HideInInspector]
	public Vector3 spawnPoint;
	Rigidbody2D rg;
	[HideInInspector]
	public MazeCreator maze;
	public bool canPlay = false;
/*	float h = 1;
	float v = 1;*/


	void Awake(){
		
		rg = GetComponent<Rigidbody2D> ();
		maze = FindObjectOfType<MazeCreator> ();
			
	}

	public void spawnPlayer(int min,int max){

		int x = Random.Range (3,maze.rows-3);
		int y = Random.Range (min,max);
		// in case the spawn point is on a wall
		if (x % 3 == 0) 
			x++;
		if (y % 3 == 0)
			y++;
		spawnPoint = new Vector3 (x, y, 0);
		transform.position = spawnPoint;
	}
	
	// Update is called once per frame
	void Update () {

		

	}

	public void walk(){
		if (!isLocalPlayer || !GameManager.startGame)
			return;
/*		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");*/
	}
	void FixedUpdate(){
/*		if (!isLocalPlayer || !GameManager.startGame)
			return;
		rg.velocity = new Vector2 (h * speed, v * speed);*/
	}


}
