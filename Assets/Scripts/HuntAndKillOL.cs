using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HuntAndKillOL : MonoBehaviour {


	MazeCreatorOL mazeSetup;
	NetworkManager mn;
	private Vector2 startPoint;
	private Cell[][] maze;
	private GameObject[][] mazeObjects;
	NetworkConnection c;
	public GameObject[] outage;
	public GameObject[] keys;
	public GameObject dogsCage;
	public int[,] wallTypes;
	[HideInInspector]
	public int rows,columns,objRows,objColumns;
	bool isRoute = true;
	bool noVictim = false;
	int direction; // 1 north 2 east 3 west 4 south


	public void Scan(){
		
		noVictim = true;
		for(int i = 0; i < rows; i++){
			for(int j =0; j < columns;j++){
				if (!maze [i][j].isVisited){
					//Debug.Log (maze [i] [j].northWall.isDestroyed);
					noVictim = false;
					// Finding a visited neighbor then destroying the wall between them and the choosing this cell to start killing from
					if(j != (columns -1) && maze[i][j+1].isVisited){ // north neighbor

						//mazeSetup.DestroyWall (maze [i] [j].northWall);
						//maze [i] [j].isVisited = true;
						//mazeSetup.DestroyWall (maze [i] [j+1].southWall);
						//maze [i] [j].destN = true;
						//maze [i] [j + 1].destS = true;
						startPoint = new Vector2 (i, j);
						isRoute = true;
						break;
					}
					else if(i != 0 && maze[i - 1][j].isVisited){ // west neighbor

					//	mazeSetup.DestroyWall (maze [i] [j].westWall);
						//maze [i] [j].isVisited = true;
					//	mazeSetup.DestroyWall (maze [i-1] [j].eastWall);
						//maze [i] [j].destW = true;
						//maze [i - 1] [j].destE = true;
						startPoint = new Vector2 (i, j);
						isRoute = true;
						break;
					}
					else if(i != (rows - 1) && maze[i+1][j].isVisited){ // east neighbor

					//	mazeSetup.DestroyWall (maze [i] [j].eastWall);
						//maze [i] [j].isVisited = true;
					//	mazeSetup.DestroyWall (maze [i+1] [j].westWall);

						//maze [i] [j].destE = true;
						//maze [i + 1] [j].destW = true;
						startPoint = new Vector2 (i, j);
						isRoute = true;
						break;
					}
					else if(j != 0 && maze[i][j -1].isVisited){ // south neighbor

					//	mazeSetup.DestroyWall (maze [i] [j].southWall);
						//maze [i] [j].isVisited = true;
					//	mazeSetup.DestroyWall (maze [i] [j -1].northWall);
						//maze [i] [j].destS = true;
						//maze [i ] [j - 1].destN = true;
						startPoint = new Vector2 (i, j);
						isRoute = true;
						break;
					}

						
				}
			}
		}

	}
		
	public void Kill(){
		

		int x = (int)startPoint.x;
		int y = (int)startPoint.y;
		//Debug.Log ("columns "+columns+" rows"+rows);
		while(isRoute){
			//isRoute = false;
			x = (int)startPoint.x;
			y = (int)startPoint.y;
			if(maze[x][y].isVisited)
				if((y == (columns - 1) || maze[x][y + 1].isVisited) &&
				(y == 0  || maze[x][y - 1].isVisited) &&
				(x == (rows -1) || maze[x + 1][y].isVisited ) &&
				(x == 0 || maze[x - 1][y].isVisited)
				){
				isRoute = false;
				//Scan ();
				break;
				}
			maze [x][y].isVisited = true;
			Destroy (maze [x] [y].cellObject);
			if (direction == 1 && y != columns - 1) {
				
				//maze [x] [y + 1].isVisited = true;
				//Debug.Log ("Killing north");
				if(!maze[x][y].destN){
					mazeSetup.DestroyWall (maze [x] [y].northWall);
					maze [x] [y].destN = true;
					maze [x] [y + 1].destS = true;
					if (!maze [x] [y + 1].isVisited) {
						startPoint = new Vector2 (x, y + 1);
					}
				}
					
			} else if (direction == 2 && x != rows - 1) {
				
				//maze [x+1] [y].isVisited = true;
				//Debug.Log ("Killing east");
				if(!maze[x][y].destE){
					mazeSetup.DestroyWall (maze [x] [y].eastWall);
					maze [x] [y].destE = true;
						maze [x + 1] [y].destW = true;
					if (!maze [x + 1] [y].isVisited) {
						startPoint = new Vector2 (x + 1, y);
					}
				}
					
			} else if (direction == 3 && x != 0) {
				
				if(!maze[x][y].destW){
					mazeSetup.DestroyWall (maze [x] [y].westWall);
					maze [x] [y].destW = true;
					maze [x - 1] [y].destE = true;
					if (!maze [x - 1] [y].isVisited) {
						startPoint = new Vector2 (x - 1, y);
					}
				}
					
			} else if (direction == 4 && y != 0) {

				if(!maze[x][y].destS){
					mazeSetup.DestroyWall (maze [x] [y].southWall);
					maze [x] [y].destS = true;
					maze [x] [y - 1].destN = true;
					if (!maze [x] [y - 1].isVisited) {
						startPoint = new Vector2 (x, y - 1);
					}
				}
			}
			// direction assignment
			direction = Random.Range (1, 5);
			//Debug.Log (direction);
			if(direction == 1 && (y == columns - 1 || maze[x][y + 1].isVisited)){
				if ( x != rows - 1 && !maze [x + 1] [y].isVisited){
					direction = 2;
					//startPoint = new Vector2 (x + 1, y);
					continue;
				}
				if ( x != 0 && !maze [x - 1] [y].isVisited){
					direction = 3;
					//startPoint = new Vector2 (x-1, y);
					continue;
				}
				if ( y != 0 && !maze [x] [y - 1].isVisited){
					direction = 4;
					//startPoint = new Vector2 (x, y-1);
					continue;
				}
			}

			else if(direction == 2 && (x == rows - 1 || maze[x + 1][y].isVisited)){
				if ( x != 0 && !maze [x - 1] [y].isVisited){
					//startPoint = new Vector2 (x-1, y);
					direction = 3;
					continue;
				}
				if (y != 0 && !maze [x] [y - 1].isVisited) {
					//startPoint = new Vector2 (x , y - 1);
					direction = 4;
					continue;
				}
				if ( y != columns - 1 && !maze [x] [y + 1].isVisited){
					//startPoint = new Vector2 (x , y + 1);
					direction = 1;
					continue;
				}
			}

			else if(direction == 3 && (x == 0 || maze [x - 1] [y].isVisited) ){
				if ( y != columns - 1 && !maze [x] [y + 1].isVisited){
					//startPoint = new Vector2 (x , y + 1);
					direction = 1;
					continue;
				}
				if ( x != rows - 1 && !maze[x + 1][y].isVisited){
					//startPoint = new Vector2 (x + 1, y);
					direction = 2;
					continue;
				}
				if ( y != 0 && !maze [x] [y - 1].isVisited){
					//startPoint = new Vector2 (x , y - 1);
					direction = 4;
					continue;
				}
			}

			else if(direction == 4 && (y == 0 || maze [x] [y - 1].isVisited)){
				if ( y != columns - 1 && !maze [x] [y + 1].isVisited){
					//startPoint = new Vector2 (x , y + 1);
					direction = 1;
					continue;
				}
				if ( x != rows - 1 && !maze[x + 1][y].isVisited){
					//startPoint = new Vector2 (x + 1 , y);
					direction = 2;
					continue;
				}
				if ( x != 0 && !maze [x - 1] [y].isVisited){
					//startPoint = new Vector2 (x - 1, y);
					direction = 3;
					continue;	
				}
			}
		}

	}
		
	Vector2 Randomize(int r1,int r2){

		var re = Random.Range (5, r1-5);
		var ce = Random.Range (5, r2-5);
		if (re % 3 == 0)
			re++;
		if (ce % 3 == 0)
			ce++;

		return new Vector2 (re, ce);
	}

	public void decideOutage(){


		int r = mazeSetup.rows;
		int c = mazeSetup.columns;
		foreach(GameObject e in outage){
			Vector2 s = Randomize(r,c);
			Vector3 ou = new Vector3 (s.x, s.y, 0);
			GameObject exit  = Instantiate (e, ou, transform.rotation);
			NetworkServer.Spawn (exit);
		}

		foreach(GameObject e in keys){

			Vector2 s = Randomize(r,c);
			Vector3 ou = new Vector3 (s.x, s.y, 0);
			GameObject key  = Instantiate (e, ou, transform.rotation);
			NetworkServer.Spawn (key);
		}

		Vector2 se = Randomize(r,c);
		Vector3 dogsPlace = new Vector3 (se.x, se.y, 0);
		GameObject dogs  = Instantiate (dogsCage, dogsPlace, transform.rotation);
		NetworkServer.Spawn (dogs);

	}

	void massacre(){
		startPoint = new Vector2 (Random.Range(0,rows), Random.Range(0,columns));
		while(!noVictim){
			if (isRoute)
				Kill ();
			else
				Scan ();
		}

		decideOutage ();
	}

	void InstantiateWall(int wallT,int i ,int j){
		
		DestroyImmediate (mazeSetup.tiles [i] [j]);
		switch(wallT){
		case 0:
			GameObject go = Instantiate (mazeSetup.specialFloor, new Vector3 (i, j, 0), Quaternion.identity);
			mazeSetup.tiles [i] [j] = go;
			break;
		case 1:
			go = Instantiate (mazeSetup.edgeLeft ,new Vector3 (i, j, 0), Quaternion.identity);
			mazeSetup.tiles [i] [j] = go;
			break;
		case 2:
			go = Instantiate (mazeSetup.verticalLeft, new Vector3 (i, j, 0), Quaternion.identity);
			mazeSetup.tiles [i] [j] = go;
			break;
		case 3:
			go = Instantiate (mazeSetup.horizontal, new Vector3 (i, j, 0), Quaternion.identity);
			mazeSetup.tiles [i] [j] = go;
			break;
		case 4:
			go = Instantiate (mazeSetup.endLeft, new Vector3 (i, j, 0), Quaternion.identity);
			mazeSetup.tiles [i] [j] = go;
			break;
		case 5:
			go = Instantiate (mazeSetup.edgeRight, new Vector3 (i, j, 0), Quaternion.identity);
			mazeSetup.tiles [i] [j] = go;
			break;
		case 6:
			go = Instantiate (mazeSetup.verticalRight, new Vector3 (i, j, 0), Quaternion.identity);
			mazeSetup.tiles [i] [j] = go;
			break;
		case 7:
			go = Instantiate (mazeSetup.endRight, new Vector3 (i, j, 0), Quaternion.identity);
			mazeSetup.tiles [i] [j] = go;
			break;
		}
	}

	void checkWalls(){
		for(int i = 3;i < objRows-3;i++){
			for(int j = 3;j < objColumns-3;j++){

				Debug.Log (i + "," + j);

				int left, right, top, bottom;
				
				left = mazeSetup.tiles [i - 1] [j].GetComponent<tileType> ().type;
				right = mazeSetup.tiles [i + 1] [j].GetComponent<tileType> ().type;
				top = mazeSetup.tiles [i] [j+1].GetComponent<tileType> ().type;
				bottom = 1;
				if(mazeSetup.tiles [i] [j-1] != null){
					bottom = mazeSetup.tiles [i] [j-1].GetComponent<tileType> ().type;
				}


				if (top == 0 && right == 0 && bottom == 0 && left == 0) {
					wallTypes [i,j] = 3;
					InstantiateWall(3,i , j);
				}

				if (top == 0 && right == 0 && bottom == 1 && left == 0) {
					// two or six

					wallTypes [i,j] = 0;
					InstantiateWall(3,i , j);

				}

				if (top == 0 && right == 1 && bottom == 0 && left == 0) {
					wallTypes [i,j] = 3;
					InstantiateWall(3,i , j);
				}

				if (top == 0 && right == 1 && bottom == 1 && left == 0) {
					wallTypes [i,j] = 1;
					InstantiateWall(1,i , j);

				}

				if (top == 0 && right == 0 && bottom == 0 && left == 1) {
					wallTypes [i,j] = 3;
					InstantiateWall(3,i , j);

				}

				if (top == 0 && right == 0 && bottom == 1 && left == 1) {
					wallTypes [i,j] = 5;
					InstantiateWall(5,i , j);

				}

				if (top == 0 && right == 1 && bottom == 0 && left == 1) {
					wallTypes [i,j] = 3;
					InstantiateWall(3,i , j);

				}

				if (top == 0 && right == 1 && bottom == 1 && left == 1) {
					// one or five

					wallTypes [i,j] = 0;
					InstantiateWall(0,i , j);

				}

				if (top == 1 && right == 0 && bottom == 0 && left == 0) {
					// 4 ou 7
					wallTypes [i,j] = 0;
					InstantiateWall(0,i , j);

				}

				if (top == 1 && right == 0 && bottom == 1 && left == 0) {

					// 2 ou 6
					wallTypes [i,j] = 0;
					InstantiateWall(0,i , j);

				}

				if (top == 1 && right == 1 && bottom == 0 && left == 0) {
					wallTypes [i,j] = 7;
					InstantiateWall(7,i , j);

				}

				if (top == 1 && right == 1 && bottom == 1 && left == 0) {
					wallTypes [i,j] = 1;
					InstantiateWall(1,i , j);

				}

				if (top == 1 && right == 0 && bottom == 0 && left == 1) {
					wallTypes [i,j] = 4;
					InstantiateWall(4,i , j);

				}

				if (top == 1 && right == 0 && bottom == 1 && left == 1) {
					wallTypes [i,j] = 5;
					InstantiateWall(5,i , j);

				}

				if (top == 1 && right == 1 && bottom == 0 && left == 1) {
					wallTypes [i,j] = 3;
					InstantiateWall(3,i , j);

				}

				if (top == 1 && right == 1 && bottom == 1 && left == 1) {
					// 1 ou 5
					wallTypes [i,j] = 0;
					InstantiateWall(0,i , j);

				}



			/*		if(mazeSetup.tiles[i][j].GetComponent<tileType>().type == -1){
					// Horizontal Wall

					if((mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 0 && mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 0 ) ||
						(mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 1 && mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 0 ) ||
						(mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 0 && mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 1 )
					){
						GameObject s = Instantiate (mazeSetup.horizontal,new Vector3(i,j,0),Quaternion.identity);
						//s.GetComponent<tileType> ().type = -1;
						DestroyImmediate (mazeSetup.tiles [i] [j].transform.gameObject);
						//NetworkServer.UnSpawn (mazeSetup.tiles [i] [j].transform.gameObject);

						mazeSetup.tiles [i] [j] = s;
					}

					// Right Corner
					else if(mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 0 
						&& mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 1 
						&& mazeSetup.tiles[i][j-1].GetComponent<tileType>().type == 0
					){
						GameObject s = Instantiate (mazeSetup.cornerLeft,new Vector3(i,j,0),Quaternion.identity);
						DestroyImmediate (mazeSetup.tiles [i] [j].transform.gameObject);
						//NetworkServer.UnSpawn (mazeSetup.tiles [i] [j].transform.gameObject);
						mazeSetup.tiles [i] [j] = s;
						NetworkServer.Spawn (s);

						int k = j-1;
						while(mazeSetup.tiles[i][k].GetComponent<tileType>().type == 0){
							Debug.Log("got in here");
							GameObject t = Instantiate (mazeSetup.verticalLeft,new Vector3(i,k,0),Quaternion.identity);
							DestroyImmediate (mazeSetup.tiles [i] [k].transform.gameObject);
							//NetworkServer.UnSpawn (mazeSetup.tiles [i] [k].transform.gameObject);
							mazeSetup.tiles [i] [k] = t;

							k--;
						}
						// Right wall for other Walls
					}
					// Left Corner
					else if(mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 1 
						&& mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 0
						&& mazeSetup.tiles[i][j-1].GetComponent<tileType>().type == 0
					){
						GameObject s = Instantiate (mazeSetup.cornerRight,new Vector3(i,j,0),Quaternion.identity);
						Destroy (mazeSetup.tiles [i] [j].transform.gameObject);
						//NetworkServer.UnSpawn (mazeSetup.tiles [i] [j].transform.gameObject);
						mazeSetup.tiles [i] [j] = s;
					
						int k = j-1;
						while(mazeSetup.tiles[i][k].GetComponent<tileType>().type == 0){
							GameObject t = Instantiate (mazeSetup.verticalRight,new Vector3(i,k,0),Quaternion.identity);
							Destroy (mazeSetup.tiles [i] [k].transform.gameObject);
							//NetworkServer.UnSpawn (mazeSetup.tiles [i] [k].transform.gameObject);
							mazeSetup.tiles [i] [k] = t;

							k--;
						}

						// Left Walls for other walls
					}
					else if(mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 1 && mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 1){
						if(mazeSetup.tiles[i][j-1].GetComponent<tileType>().type == 1){
							GameObject s = Instantiate (mazeSetup.endLeft,new Vector3(i,j,0),Quaternion.identity);
							Destroy (mazeSetup.tiles [i] [j].transform.gameObject);
							//NetworkServer.UnSpawn (mazeSetup.tiles [i] [j].transform.gameObject);
							mazeSetup.tiles [i] [j] = s;

						}
						else{
							GameObject s = Instantiate (mazeSetup.endRight,new Vector3(i,j,0),Quaternion.identity);
							DestroyImmediate (mazeSetup.tiles [i] [j].transform.gameObject);
							mazeSetup.tiles [i] [j] = s;
						}
					}
				}*/

			
			}
		}
	}

	// Use this for initialization
	void Start() {
		direction = (int) Random.Range (1, 4);
		mazeSetup = GameObject.FindGameObjectWithTag("MazeCreator").GetComponent<MazeCreatorOL> ();
		mazeObjects = mazeSetup.tiles;
		rows = (mazeSetup.rows - 1) / 3;
		objRows = mazeSetup.rows;
		columns = (mazeSetup.columns - 1) / 3;
		objColumns = mazeSetup.columns;
		wallTypes = new int[objRows,objColumns];
		Debug.Log (objRows+" "+objColumns);
		mazeSetup.instantiateMaze ();
		maze = mazeSetup.cells;
		massacre ();
		//checkWalls ();

	}
/*	public  void Start() {

		mazeSetup = FindObjectOfType<MazeCreatorOL> ();
		mazeObjects = mazeSetup.tiles;
		//mazeSetup = GameObject.FindGameObjectWithTag("MazeCreator").GetComponent<MazeCreator> ();
		rows = (mazeSetup.rows - 1) / 3;
		objRows = mazeSetup.rows;
		columns = (mazeSetup.columns - 1) / 3;
		objColumns = mazeSetup.columns;
		mazeSetup.instantiateMaze ();
		maze = mazeSetup.cells;
		massacre ();
		checkWalls ();
		//NetworkServer.Spawn (mazeSetup.mazeHolder);
	}*/

}
