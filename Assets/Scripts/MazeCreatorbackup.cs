/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MazeCreator : NetworkBehaviour
{

	public int columns = 40;
	public int rows = 40;
	public GameObject floorType;
	public GameObject wallType;
	public GameObject wallLeftType;
	public GameObject wallRightType;
	public GameObject wallDownType;
	public GameObject wallHorizontalType;
	public GameObject wallvertRightType;
	public GameObject wallvertLeftType;
	public GameObject specialFloorType;
	public GameObject wallEdgeVerticalRightyType;
	public GameObject wallEdgeVerticalLeftyType;
	public GameObject wallEdgeHorizoDownyType;
	public GameObject wallEdgeHorinzonUppyType;
	public GameObject cornerTopLeftType;
	public GameObject cornerTopRightType;
	public GameObject cornerBotTopRightType;
	public GameObject cornerBotLeftType;
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

	public void instantiateMaze(){
		if(mazeHolder == null){
			Debug.Log ("nulllllll");
		}
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
		Vector3 pos = new Vector3 (0, 0, 0);
		GameObject go;
		go = Instantiate (wallEdgeVerticalRightyType, pos, Quaternion.identity) as GameObject;
		go.name = "Wall" + 0 +","+ 0;
		go.GetComponent<tileType> ().type = -1;
		//NetworkServer.Spawn (go);
		tiles [0][0] = go;
		tiles [0] [0].transform.SetParent (mazeHolder.transform);
		// corner
		pos = new Vector3 (0, columns -1, 0);
		go = Instantiate (wallEdgeVerticalRightyType, pos, Quaternion.identity) as GameObject;
		go.name = "Wall" + 0 +","+ (columns -1);
		go.GetComponent<tileType> ().type = -1;
		//NetworkServer.Spawn (go);
		tiles [0][columns -1] = go;
		tiles [0] [columns -1].transform.SetParent (mazeHolder.transform);
		// corner
		pos = new Vector3 (rows -1 , 0, 0);
		go = Instantiate (wallEdgeVerticalRightyType, pos, Quaternion.identity) as GameObject;
		go.name = "Wall" + (rows -1) +","+ 0;
		go.GetComponent<tileType> ().type = -1;
		//NetworkServer.Spawn (go);
		tiles [rows -1][0] = go;
		tiles [rows -1] [0].transform.SetParent (mazeHolder.transform);
		// corner
		pos = new Vector3 (rows -1 , columns -1, 0);
		go = Instantiate (wallEdgeVerticalRightyType, pos, Quaternion.identity) as GameObject;
		go.name = "Wall" + (rows -1) +","+ (columns -1);
		go.GetComponent<tileType> ().type = -1;
		//NetworkServer.Spawn (go);
		pos = new Vector3 (rows -1 , columns -1, 0);
		tiles [rows -1][columns -1] = go;
		tiles [rows -1] [columns -1].transform.SetParent (mazeHolder.transform);

		for(int j = 1; j < columns - 1;j++){
			Vector3 position = new Vector3 (0, j, 0);
			GameObject goInstance;
			goInstance = Instantiate (wallEdgeVerticalRightyType, position, Quaternion.identity) as GameObject;
			goInstance.name = "Wall" + 0 +","+ j;
			goInstance.GetComponent<tileType> ().type = -1;
			//NetworkServer.Spawn (goInstance);
			tiles [0][j] = goInstance;
			tiles [0] [j].transform.SetParent (mazeHolder.transform);
		}
		for(int j = 1; j < columns - 1;j++){
			Vector3 position = new Vector3 (rows-1, j, 0);
			GameObject goInstance;
			goInstance = Instantiate (wallEdgeVerticalLeftyType, position, Quaternion.identity) as GameObject;
			//NetworkServer.Spawn (goInstance);
			goInstance.name = "Wall" + rows +","+ j;
			goInstance.GetComponent<tileType> ().type = -1;
			tiles [rows-1][j] = goInstance;
			tiles [rows-1] [j].transform.SetParent (mazeHolder.transform);
		}
		for(int i = 1; i < rows - 1;i++){
			Vector3 position = new Vector3 (i, 0, 0);
			GameObject goInstance;
			goInstance = Instantiate (wallEdgeHorizoDownyType, position, Quaternion.identity) as GameObject;
			goInstance.name = "Wall" + i +","+ 0;
			goInstance.GetComponent<tileType> ().type = -1;
			//NetworkServer.Spawn (goInstance);
			tiles [i][0] = goInstance;
			tiles [i] [0].transform.SetParent (mazeHolder.transform);
		}
		for(int i = 1; i < rows - 1;i++){
			Vector3 position = new Vector3 (i, columns-1, 0);
			GameObject goInstance;
			goInstance = Instantiate (wallEdgeHorinzonUppyType, position, Quaternion.identity) as GameObject;
			goInstance.name = "Wall" + i +","+ columns;
			goInstance.GetComponent<tileType> ().type = -1;
			//NetworkServer.Spawn (goInstance);
			tiles [i][columns-1] = goInstance;
			tiles [i] [columns-1].transform.SetParent (mazeHolder.transform);
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
					goInstance = Instantiate (wallvertLeftType, position, Quaternion.identity) as GameObject;
					goInstance.name = "Wall" + i +","+ j;
					goInstance.GetComponent<tileType> ().type = -1;
					//NetworkServer.Spawn (goInstance);
					tiles [i][j] = goInstance;
					tiles [i] [j].transform.SetParent (mazeHolder.transform);
					walls.Add(wall);
				}
				else{
					if(j % 3 == 0){
						if((i+1) % 3 != 0){ // this test is to avoid duplicate walls
							Wall wall = new Wall() ;
							Vector3 position2 = new Vector3 (i+1, j, 0);
							wall.createWall (new Vector2 (i, j), WallOrientation.Horizontal);
							goInstance = Instantiate (wallHorizontalType, position, Quaternion.identity) as GameObject;
							goInstance.name = "Wall" + i +","+ j;
							goInstance.GetComponent<tileType> ().typee = 0;
							//NetworkServer.Spawn (goInstance);
							tiles [i][j] = goInstance;

							goInstance = Instantiate (wallHorizontalType, position2, Quaternion.identity) as GameObject;
							goInstance.name = "Wall" + i + 1 +","+ j;
							goInstance.GetComponent<tileType> ().typee = 0;
							//NetworkServer.Spawn (goInstance);
							tiles [i+1][j] = goInstance;

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
							Vector3 position2 = new Vector3 (i, j+1, 0);
							wall.createWall (new Vector2 (i, j), WallOrientation.Vertical);
							goInstance = Instantiate (wallvertLeftType, position, Quaternion.identity) as GameObject;
							goInstance.name = "Wall" + i +","+ j;
							goInstance.GetComponent<tileType> ().typee = 1;
							//NetworkServer.Spawn (goInstance);
							tiles [i][j] = goInstance;

							goInstance = Instantiate (wallvertLeftType, position2, Quaternion.identity) as GameObject;
							goInstance.name = "Wall" + i +","+ j;
							goInstance.GetComponent<tileType> ().typee = 1;
							//NetworkServer.Spawn (goInstance);
							tiles [i][j+1] = goInstance;
							wall.tiles [0] = new Vector2 (i, j); // add the two corresponding tiles since each wall has 2 implicit tiles.
							wall.tiles [1] = new Vector2(i,j+1);
							tiles [i] [j].transform.SetParent (mazeHolder.transform);
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
					/*goInstance = Instantiate (wallType, position, Quaternion.identity) as GameObject;
					goInstance.name = "Tile" + i +","+ j;
					if(j % 3 == 0 && i % 3 == 0){
						goInstance.GetComponent<tileType> ().type = -1;
					}
					tiles [i][j] = goInstance;*/
/*				}
				else{
					goInstance = Instantiate (floorType, position, Quaternion.identity) as GameObject;
					goInstance.name = "Tile" + i +","+ j;
					goInstance.transform.parent = mazeHolder.transform;
					//NetworkServer.Spawn (goInstance);
					tiles [i][j] = goInstance;
					tiles [i] [j].transform.SetParent (mazeHolder.transform);
				}

				if((i % 3 != 0 && j % 3 !=0) && (i % 2 == 0 && j % 2 == 0)){ // a cell
					pos = new Vector2 (i, j);
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


					goInstance = Instantiate (specialFloorType, position, Quaternion.identity) as GameObject;
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
	}*/

/*	public void DestroyWall(Wall wall){

		GameObject temp1 = tiles[(int)wall.tiles[0].x][(int)wall.tiles[0].y];
		GameObject temp2 = tiles[(int)wall.tiles[1].x][(int)wall.tiles[1].y];
		tiles [(int)wall.tiles [0].x] [(int)wall.tiles [0].y].GetComponent<tileType> ().type = 1;
		tiles [(int)wall.tiles [1].x] [(int)wall.tiles [1].y].GetComponent<tileType> ().type = 1;
		Destroy (temp1);
		//NetworkServer.UnSpawn (temp1);
		Destroy (temp2);
		//NetworkServer.UnSpawn (temp2);
		//Debug.Log ("Destroying tile"+wall.tiles[0].x+wall.tiles[0].y+"and "+wall.tiles[1].x+wall.tiles[1].y);
		// Replace the walls with floor tiles.
		temp1 = Instantiate (floorType, wall.tiles[0], Quaternion.identity) as GameObject;
		temp1.name = "Tile" + (int)wall.tiles[0].x +","+ (int)wall.tiles[0].y;
		temp1.transform.SetParent (mazeHolder.transform);
		//NetworkServer.Spawn (temp1);
		temp2 = Instantiate (floorType, wall.tiles[1], Quaternion.identity) as GameObject;
		temp2.name = "Tile" + (int)wall.tiles[1].x +","+ (int)wall.tiles[1].y;
		temp2.transform.SetParent (mazeHolder.transform);
		//NetworkServer.Spawn (temp2);
		//wallObjects[0] = temp1;
		//wallObjects[1] = temp2;
	}

	public void createWall(Wall wall){
		GameObject temp1 = tiles[(int)wall.tiles[0].x][(int)wall.tiles[0].y];
		GameObject temp2 = tiles[(int)wall.tiles[1].x][(int)wall.tiles[1].y];
		Destroy (temp1);
		Destroy (temp2);
		//Debug.Log ("Destroying tile"+wall.tiles[0].x+wall.tiles[0].y+"and "+wall.tiles[1].x+wall.tiles[1].y);
		// Replace the walls with floor tiles.
		temp1 = Instantiate (wallType, wall.tiles[0], Quaternion.identity) as GameObject;
		temp1.name = "Tile Created New" + (int)wall.tiles[0].x + (int)wall.tiles[0].y;
		temp1.transform.SetParent (mazeHolder.transform);
		temp2 = Instantiate (wallType, wall.tiles[1], Quaternion.identity) as GameObject;
		temp2.name = "Tile Created New" + (int)wall.tiles[1].x + (int)wall.tiles[1].y;
		temp2.transform.SetParent (mazeHolder.transform);
	}
		
}*/