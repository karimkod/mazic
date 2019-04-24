using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerOL : MonoBehaviour {
	
	public float speed = 3.5f;
	[HideInInspector]
	public Vector3 spawnPoint;

	[HideInInspector]
	public MazeCreator maze;



	void Awake(){
		

		maze = GameObject.FindGameObjectWithTag ("MazeCreator").GetComponent<MazeCreator>();	
			
	}

	public void spawnPlayer(int min,int max){

		int x = Random.Range (0,maze.rows);
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

	}




}
