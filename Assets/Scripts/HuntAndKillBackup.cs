/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HuntAndKill : NetworkBehaviour {


	MazeCreator mazeSetup;
	NetworkManager mn;
	private Vector2 startPoint;
	private Cell[][] maze;
	private GameObject[][] mazeObjects;
	NetworkConnection c;
	public GameObject[] outage;
	public GameObject[] keys;
	public GameObject dogsCage;
 	[HideInInspector]
	public int rows,columns,objRows,objColumns;
	bool isRoute = true;
	bool noVictim = false;



	public void Scan(){
		
		noVictim = true;
		for(int i = 0; i < rows; i++){
			for(int j =0; j < columns;j++){
				if (!maze [i][j].isVisited){
					noVictim = false;
					// Finding a visited neighbor then destroying the wall between them and the choosing this cell to start killing from
					if(i != 0 && maze[i - 1][j].isVisited){ // west neighbor
						//mazeSetup.DestroyWall (maze [i] [j].westWall);
						startPoint = new Vector2 (i, j);
						isRoute = true;
						break;
					}
					else if(i != (rows - 1) && maze[i+1][j].isVisited){ // east neighbor
						//mazeSetup.DestroyWall (maze [i] [j].eastWall);
						startPoint = new Vector2 (i, j);
						isRoute = true;
						break;
					}
					else if(j != 0 && maze[i][j -1].isVisited){ // south neighbor
						//mazeSetup.DestroyWall (maze [i] [j].southWall);
						startPoint = new Vector2 (i, j);
						isRoute = true;
						break;
					}
					else if(j != (columns -1) && maze[i][j+1].isVisited){ // north neighbor
						//mazeSetup.DestroyWall (maze [i] [j].northWall);
						startPoint = new Vector2 (i, j);
						isRoute = true;
						break;
					}
				}
			}
		}
	}
		
	public void Kill(){
		
		int direction = (int) Random.Range (1, 4); // 1 north 2 east 3 west 4 south
		int x = (int)startPoint.x;
		int y = (int)startPoint.y;
		//Debug.Log ("columns "+columns+" rows"+rows);
		while(isRoute){
			//isRoute = false;
			x = (int)startPoint.x;
			y = (int)startPoint.y;
			/*Debug.Log ("Direction =" + direction);
			Debug.Log ("x: " + x + " , y: " + y);*/
			//Debug.Log(" maze is visited : "+maze[x][y+1].isVisited);
			//Debug.Log ("direction :" + direction);
/*			if(maze[x][y].isVisited)
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
			if(direction == 1 && y != columns - 1){
				mazeSetup.DestroyWall(maze[x][y].northWall);
				//maze [x] [y + 1].isVisited = true;
				//Debug.Log ("Killing north");
				if(!maze[x][y + 1].isVisited )
					startPoint = new Vector2 (x, y + 1);
			}
			else if(direction == 2 && x != rows - 1){
				mazeSetup.DestroyWall (maze[x][y].eastWall);
				//maze [x+1] [y].isVisited = true;
				//Debug.Log ("Killing east");
				if(!maze[x + 1][y].isVisited )
					startPoint = new Vector2 (x + 1, y);
			}
			else if(direction == 3 && x != 0){
				mazeSetup.DestroyWall (maze[x][y].westWall);
				//maze [x-1] [y].isVisited = true;
				//Debug.Log ("Killing west");
				if(!maze[x - 1 ][y].isVisited)
					startPoint = new Vector2 (x - 1 , y);
			}
			else if(direction == 4 && y != 0){
				mazeSetup.DestroyWall (maze[x][y].southWall);
				//maze [x] [y - 1].isVisited = true;
				//Debug.Log ("Killing south");
				if(!maze[x][y - 1].isVisited)
					startPoint = new Vector2 (x, y - 1);
			}


			direction = Random.Range (1, 5);
			if(direction == 1 && (y == columns - 1 || maze[x][y + 1].isVisited)){
				if ( x != rows - 1 && !maze [x + 1] [y].isVisited){
					direction = 2;
					continue;
				}
				if ( x != 0 && !maze [x - 1] [y].isVisited){
					direction = 3;
					continue;
				}
				if ( y != 0 && !maze [x] [y - 1].isVisited){
					direction = 4;
					continue;
				}
			}

			else if(direction == 2 && (x == rows - 1 || maze[x + 1][y].isVisited)){
				if ( y != columns - 1 && !maze [x] [y + 1].isVisited){
					direction = 1;
					continue;
				}
				if ( x != 0 && !maze [x - 1] [y].isVisited){
					direction = 3;
					continue;
				}
				if (y != 0 && !maze [x] [y - 1].isVisited) {
					direction = 4;
					continue;
				}
			}

			else if(direction == 3 && (x == 0 || maze [x - 1] [y].isVisited) ){
				if ( y != columns - 1 && !maze [x] [y + 1].isVisited){
					direction = 1;
					continue;
				}
				if ( x != rows - 1 && !maze[x + 1][y].isVisited){
					direction = 2;
					continue;
				}
				if ( y != 0 && !maze [x] [y - 1].isVisited){
					direction = 4;
					continue;
				}
			}

			else if(direction == 4 && (y == 0 || maze [x] [y - 1].isVisited)){
				if ( y != columns - 1 && !maze [x] [y + 1].isVisited){
					direction = 1;
					continue;
				}
				if ( x != rows - 1 && !maze[x + 1][y].isVisited){
					direction = 2;
					continue;
				}
				if ( x != 0 && !maze [x - 1] [y].isVisited){
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

	void checkWalls(){
		for(int i = 1;i < objRows-1;i++){
			for(int j = 1;j < objColumns-1;j++){

				if(mazeSetup.tiles[i][j].GetComponent<tileType>().type == -1){
					// Horizontal Wall

					if((mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 0 && mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 0 ) ||
						(mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 1 && mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 0 ) ||
						(mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 0 && mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 1 )
					){
						GameObject s = Instantiate (mazeSetup.wallHorizontalType,new Vector3(i,j,0),Quaternion.identity);
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
						GameObject s = Instantiate (mazeSetup.wallLeftType,new Vector3(i,j,0),Quaternion.identity);
						DestroyImmediate (mazeSetup.tiles [i] [j].transform.gameObject);
						//NetworkServer.UnSpawn (mazeSetup.tiles [i] [j].transform.gameObject);
						mazeSetup.tiles [i] [j] = s;

						int k = j-1;
						while(mazeSetup.tiles[i][k].GetComponent<tileType>().type == 0){
							Debug.Log("got in here");
							GameObject t = Instantiate (mazeSetup.wallvertRightType,new Vector3(i,k,0),Quaternion.identity);
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
						GameObject s = Instantiate (mazeSetup.wallRightType,new Vector3(i,j,0),Quaternion.identity);
						Destroy (mazeSetup.tiles [i] [j].transform.gameObject);
						//NetworkServer.UnSpawn (mazeSetup.tiles [i] [j].transform.gameObject);
						mazeSetup.tiles [i] [j] = s;

						int k = j-1;
						while(mazeSetup.tiles[i][k].GetComponent<tileType>().type == 0){
							GameObject t = Instantiate (mazeSetup.wallvertLeftType,new Vector3(i,k,0),Quaternion.identity);
							Destroy (mazeSetup.tiles [i] [k].transform.gameObject);
							//NetworkServer.UnSpawn (mazeSetup.tiles [i] [k].transform.gameObject);
							mazeSetup.tiles [i] [k] = t;

							k--;
						}

						// Left Walls for other walls
					}
					else if(mazeSetup.tiles[i+1][j].GetComponent<tileType>().type == 1 && mazeSetup.tiles[i-1][j].GetComponent<tileType>().type == 1){
						if(mazeSetup.tiles[i][j-1].GetComponent<tileType>().type == 1){
							GameObject s = Instantiate (mazeSetup.floorType,new Vector3(i,j,0),Quaternion.identity);
							Destroy (mazeSetup.tiles [i] [j].transform.gameObject);
							//NetworkServer.UnSpawn (mazeSetup.tiles [i] [j].transform.gameObject);
							mazeSetup.tiles [i] [j] = s;

						}
						/*else{
							GameObject s = Instantiate (mazeSetup.wallVerticalType,new Vector3(i,j,0),Quaternion.identity);
							DestroyImmediate (mazeSetup.tiles [i] [j].transform.gameObject);
							mazeSetup.tiles [i] [j] = s;
						}*/
/*					}
				}


			}
		}
	}
	// Use this for initialization
	public override void OnStartServer() {

		mazeSetup = FindObjectOfType<MazeCreator> ();
		mazeObjects = mazeSetup.tiles;
		//mazeSetup = GameObject.FindGameObjectWithTag("MazeCreator").GetComponent<MazeCreator> ();
		rows = (mazeSetup.rows - 1) / 3;
		objRows = mazeSetup.rows;
		columns = (mazeSetup.columns - 1) / 3;
		objColumns = mazeSetup.columns;
		mazeSetup.instantiateMaze ();
		maze = mazeSetup.cells;
		massacre ();

		NetworkServer.Spawn (mazeSetup.mazeHolder);
	}

}*/
