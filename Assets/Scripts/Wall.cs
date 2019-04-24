using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallOrientation  {
	Vertical,
	Horizontal,
	Center
}

public class Wall : MonoBehaviour{
	//bool isBorder;
	public WallOrientation orientation;
	public Vector2 position;
	public Vector2[] tiles = new Vector2[2];
	public GameObject[] wallObjects;
	public GameObject floor;
	public bool isDestroyed;
	public int index;
	// Use this for initialization

	public void createWall(Vector2 pos,WallOrientation ori){
		isDestroyed = false;
		position = pos;
		orientation = ori;

		if (orientation == WallOrientation.Horizontal)
			index = (int)position.y;
		else
			index = (int)position.x;

		//Debug.Log ("Wall index" + index);
	}
	public void DestroyWall(){
		isDestroyed = false;
		GameObject temp1 = wallObjects[0];
		GameObject temp2 = wallObjects[1];
		Destroy (temp1);
		Destroy (temp2);
		// Replace the walls with floor tiles.
		/*temp1 = Instantiate (floor, position, Quaternion.identity) as GameObject;
		temp1.name = "Tile" + (int)tiles[0].x + (int)tiles[0].y;
		temp2 = Instantiate (floor, position, Quaternion.identity) as GameObject;
		temp2.name = "Tile" + (int)tiles[0].x + (int)tiles[0].y;
		wallObjects[0] = temp1;
		wallObjects[1] = temp2;*/

	}
	void Start () {
	/*	maze.instantiateMaze ();
		floor = maze.floorType;
		wallObjects[0] = maze.tiles[(int)tiles[0].x][(int)tiles[0].y];
		wallObjects[1] = maze.tiles[(int)tiles[1].x][(int)tiles[1].y];
		if(wallObjects[0] == maze.tiles[(int)tiles[0].x][(int)tiles[0].y]){
			Debug.Log ("shit");
		} */
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
