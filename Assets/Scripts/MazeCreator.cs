using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MazeCreator : MonoBehaviour
{

	public int columns = 40;
	public int rows = 40;
	public GameObject[] floors;
	int[] floorType = {0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,2,2,2,3,3,3,0,0,0,4,4,4,0,0,0,5,5,5,0,0,0,6,6,6,0,0,0,7,7,7,0,0,0,8,8,8,0,0,0,
		9,9,9,0,0,0,10,10,10,0,0,0,11,11,11,0,0,0,12,12,12,0,0,0,13,13,13,0,0,0,14,14,14,0,0,0,15,15,15,0,0,0,16,16,16,0,0,0,17,17,17,0,0,0};
	public GameObject wallType;
	public GameObject endLeft;
	public GameObject endRight;
	public GameObject horizontal;
	public GameObject edgeLeft;
	public GameObject edgeRight;
	public GameObject verticalRight;
	public GameObject borderRight;
	public GameObject borderLeft;
	public GameObject borderTop;
	public GameObject borderDown;
	public GameObject verticalLeft;
	public GameObject specialFloor;
	public GameObject cornerRight;
	public GameObject cornerLeft;
	public GameObject cornerTopLeft;
	public GameObject cornerTopRight;
	/*public GameObject endBotRight;
	public GameObject endBotLeft;*/
	public GameObject[][] tiles;
	public Cell[][] cells;
	private List<Wall> walls = new List <Wall>();
	[HideInInspector]
	public GameObject mazeHolder;

	void Awake(){
		//DontDestroyOnLoad (this.gameObject);
		mazeHolder = GameObject.FindGameObjectWithTag ("Maze");
		//DontDestroyOnLoad (mazeHolder);
	}

	void InstantiateTile(GameObject tile,int i, int j,int type){
		Vector3 pos = new Vector3 (i, j, 0);
		GameObject go;
		go = Instantiate (tile, pos, Quaternion.identity) as GameObject;
		go.name = "Tile" + i +","+ j;
		go.GetComponent<tileType> ().type = type;
		NetworkServer.Spawn (go);
		//NetworkServer.Spawn (go);
		tiles [i][j] = go;
		tiles [i] [j].transform.SetParent (mazeHolder.transform);
	}

	public void instantiateMaze(){

		tiles = new GameObject[rows][];
		cells = new Cell[(rows-1) / 3][];
		for(int i =0; i < (rows-1) / 3;i++){
			cells [i] = new Cell[(columns - 1) / 3];
		}
		for (int i = 0; i < rows; i++) {
			tiles [i] = new GameObject[columns]; // assign columns :3
		}

		int ii = 0; // cells row index
		int jj = 0; // cells column index
		// corner
		InstantiateTile(cornerTopLeft,0,0,0);
		// corner
		InstantiateTile(cornerTopRight,0,columns -1,0);
		// corner
		InstantiateTile(cornerLeft,rows -1,0,0);
		// corner
		InstantiateTile(cornerRight,rows -1,columns -1,0);

		// right walll
		for(int j = 1; j < columns - 1;j++){
			InstantiateTile(borderRight,0,j,0);
		}
		// left walll
		for(int j = 1; j < columns - 1;j++){
			InstantiateTile(borderLeft,rows-1,j,0);
		}
		// bot walll
		for(int i = 1; i < rows - 1;i++){
			InstantiateTile(borderDown,i,0,0);
		}
		for(int i = 1; i < rows - 1;i++){
			InstantiateTile(borderTop,i,columns-1,0);
		}


		//// NORMAL WALLS 
		for (int i = 1; i < rows -1; i++) { // identify and store walls 
			for (int j = 1; j < columns -1; j++) {
				Vector3 position = new Vector3 (i, j, 0);
				GameObject goInstance;
				if (i % 3 == 0 && j % 3 == 0 ) { // a center wall
					//Debug.Log("Center Wall Found at "+i+","+j);
					Wall wall = new Wall();
					wall.createWall(new Vector2(i,j),WallOrientation.Center);
					InstantiateTile (horizontal, i, j, -1);
					walls.Add(wall);
				}
				else{
					if(j % 3 == 0){
						if((i+1) % 3 != 0){ // this test is to avoid duplicate walls
							Wall wall = new Wall() ;
							Vector3 position2 = new Vector3 (i+1, j, 0);
							InstantiateTile (horizontal, i, j, 0);


							wall.createWall (new Vector2 (i, j), WallOrientation.Horizontal);
							InstantiateTile (horizontal, i+1, j, 0);

							//NetworkServer.Spawn (goInstance);

							wall.tiles [0] = new Vector2 (i, j);
							wall.tiles [1] = new Vector2 (i + 1, j);
							tiles [i] [j].transform.SetParent (mazeHolder.transform);
							//Debug.Log("Wall Found at "+i+","+j+ "/ Horizontal index "+wall.index);
							//Debug.Log ("wall tiles" + i + j + " ," + (i + 1) + j);
							walls.Add(wall);
						}
					}
					if(i % 3 == 0){
						if((j+1) % 3 != 0){ // this test for avoiding duplicate walls
							Wall wall = new Wall() ;
							//InstantiateTile (horizontal, i, j, 0);
							Vector3 position2 = new Vector3 (i, j+1, 0);
							wall.createWall (new Vector2 (i, j), WallOrientation.Vertical);
							InstantiateTile (horizontal, i, j, 0);


							InstantiateTile (horizontal, i, j+1, 0);

							wall.tiles [0] = new Vector2 (i, j); // add the two corresponding tiles since each wall has 2 implicit tiles.
							wall.tiles [1] = new Vector2(i,j+1);
							//Debug.Log("Wall Found at "+i+","+j+ "/ Vertical index "+wall.index);
							//Debug.Log ("wall tiles" + i + j + " ," + i + (j+1));
							walls.Add(wall);
						}
					}
				}

			}
		}
		//Debug.Log ("Wall Count" + walls.Count);
		for(int i = 0;i < rows; i++){

			for(int j=0; j < columns; j++){
				//tiles [i] [j] = new GameObject ("Tile "+i+j);
				Vector3 position = new Vector3 (i, j, 0);
				GameObject goInstance;
				if (j % 3 == 0 || i % 3 == 0){
				}
				else{
					int [] ra = {5,8,7};
					int c = Random.Range(0,3);
					if(i % ra[c] == 0 || j % ra[c] == 0){
						InstantiateTile (floors[Random.Range(1,floors.Length)], i, j, 1);
					}
					else{
						InstantiateTile (floors[0], i, j, 1);
					}
					/*int type = Random.Range (0, floorType.Length);
					InstantiateTile (floors[floorType[type]], i, j, 1);*/
				}

				if((i % 3 != 0 && j % 3 !=0) && (i % 2 == 0 && j % 2 == 0)){ // a cell
					Vector3 pos = new Vector2 (i, j);
					Cell c = new Cell (pos);

					foreach(Wall w in walls){
						//Debug.Log ("Wall n°" + w.index + " " + w.orientation);
						if (w.orientation == WallOrientation.Vertical && (w.index == i - 1 || w.index == i - 2) && (j >= w.position.y) )
							c.westWall = w;
						if (w.orientation == WallOrientation.Vertical && (w.index == i + 1 || w.index == i + 2) && (j >= w.position.y) )
							c.eastWall = w;
						if (w.orientation == WallOrientation.Horizontal && (w.index == j + 1 || w.index == j + 2) && (i >= w.position.x))
							c.northWall = w;
						if (w.orientation == WallOrientation.Horizontal && (w.index == j - 1 || w.index == j - 2) && (i >= w.position.x))
							c.southWall = w;
					}


					goInstance = Instantiate (specialFloor, position, Quaternion.identity) as GameObject;
					goInstance.transform.parent = mazeHolder.transform;
					//NetworkServer.Spawn (goInstance);
					c.cellObject = goInstance;
					cells [ii] [jj] = c;
					jj++;
					if (jj == ((columns - 1) / 3)){
						ii++;
						jj = 0;
					}
				}


			}
		}	
	}

	public void DestroyWall(Wall wall){

		GameObject temp1 = tiles[(int)wall.tiles[0].x][(int)wall.tiles[0].y];
		GameObject temp2 = tiles[(int)wall.tiles[1].x][(int)wall.tiles[1].y];
		tiles [(int)wall.tiles [0].x] [(int)wall.tiles [0].y].GetComponent<tileType> ().type = 1;
		tiles [(int)wall.tiles [1].x] [(int)wall.tiles [1].y].GetComponent<tileType> ().type = 1;


		Destroy (temp1);
		//NetworkServer.UnSpawn (temp1);
		Destroy (temp2);
		NetworkServer.UnSpawn (temp1);
		NetworkServer.UnSpawn (temp2);
		int [] ra = {5,8,7};
		int c = Random.Range(0,3);
	/*	if((int)wall.tiles [0].x % ra[c] == 0 || (int)wall.tiles [0].y % ra[c] == 0){
			InstantiateTile (floors[Random.Range(1,floors.Length)], (int)wall.tiles[0].x, (int)wall.tiles[0].y, 1);
		}*/
	//	else{
			InstantiateTile (floors[0], (int)wall.tiles[0].x, (int)wall.tiles[0].y, 1);
	//	}


		/*if((int)wall.tiles [1].x % ra[c] == 0 || (int)wall.tiles [1].y % ra[c] == 0){
			InstantiateTile (floors[Random.Range(1,floors.Length)], (int)wall.tiles[1].x, (int)wall.tiles[1].y, 1);
		}*/
		//else{
			InstantiateTile (floors[0], (int)wall.tiles[1].x, (int)wall.tiles[1].y, 1);
		//}
		//NetworkServer.UnSpawn (temp2);
		//Debug.Log ("Destroying tile"+wall.tiles[0].x+wall.tiles[0].y+"and "+wall.tiles[1].x+wall.tiles[1].y);
		// Replace the walls with floor tiles.
		//InstantiateTile (floor, (int)wall.tiles[0].x, (int)wall.tiles[0].y, 1);
		//NetworkServer.Spawn (temp1);

		//NetworkServer.Spawn (temp2);
		//wallObjects[0] = temp1;
		//wallObjects[1] = temp2;
	}

	public void createWall(Wall wall){
		GameObject temp1 = tiles[(int)wall.tiles[0].x][(int)wall.tiles[0].y];
		GameObject temp2 = tiles[(int)wall.tiles[1].x][(int)wall.tiles[1].y];
		tiles [(int)wall.tiles [0].x] [(int)wall.tiles [0].y].GetComponent<tileType> ().type = 0;
		tiles [(int)wall.tiles [1].x] [(int)wall.tiles [1].y].GetComponent<tileType> ().type = 0;
		//Destroy (temp1);
		//Destroy (temp2);
		//Debug.Log ("Destroying tile"+wall.tiles[0].x+wall.tiles[0].y+"and "+wall.tiles[1].x+wall.tiles[1].y);
		// Replace the walls with floor tiles.
		InstantiateTile (verticalLeft, (int)wall.tiles[0].x, (int)wall.tiles[0].y, 1);
		InstantiateTile (verticalLeft, (int)wall.tiles[1].x, (int)wall.tiles[1].y, 1);
	}


}