using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Cell : NetworkBehaviour{

	public Vector2 tilePosition;
	public GameObject cellObject;
	public bool isVisited = false;
	public Wall northWall;
	public Wall southWall;
	public Wall eastWall;
	public Wall westWall;
	public bool destN;
	public bool destW;
	public bool destE;
	public bool destS;

	public Cell(Vector2 pos){
		tilePosition = pos;

	}
}
