using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{
	North,
	South,
	East,
	West
};
public class Patroller : MonoBehaviour {


	int h = 1;
	int v = 1;
	public int speed = 1;
	public Direction direction,lastDirection;
	BoxCollider2D collider;
	Rigidbody2D rg;
	GameObject[] runners;

	void Start () {
		
		runners = GameObject.FindGameObjectsWithTag ("Runner");
		collider = GetComponent<BoxCollider2D> ();
		direction = (Direction) Random.Range (0, 4);
		lastDirection = direction;
		DecideRoute ();
		rg = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		if(Time.frameCount % 200 == 0){
			DecideRoute ();
		}

		foreach(GameObject r in runners){
			float dist = Vector2.Distance (transform.position, r.gameObject.transform.position);
			if(dist <= 4){
                Debug.Log("i caught a fucking runner");

				DestroyImmediate (this.gameObject);
				// run animation
			}	
		}
	}

	void FixedUpdate(){
		rg.velocity = new Vector2 (h * speed, v * speed);
	}

	void DecideRoute(){
		
		int[] noNorth = new int[] { 1, 2, 3 };
		int[] noSouth = new int[] { 0, 2, 3 };
		int[] noEast = new int[] { 0, 1, 3 };
		int[] noWest = new int[] { 0, 1, 2 };

		if(direction == Direction.North){
			
			direction = (Direction) noNorth [Random.Range (0, noNorth.Length)];
			
			if (direction == Direction.South){
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1.0f,1.5f);
				collider.offset = new Vector2 (0f, -0.5f);
				MoveSouth ();
				lastDirection = Direction.South;
			}
			if (direction == Direction.East) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1.5f,1.0f);
				collider.offset = new Vector2 (0.5f, 0f);
				MoveEast ();
				lastDirection = Direction.East;
			}
			if (direction == Direction.West) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1.5f,1.0f);
				collider.offset = new Vector2 (-0.5f, 0f);
				MoveWest ();
				lastDirection = Direction.West;
			}
		}

		else if(direction == Direction.South){
			
			direction = (Direction) noSouth [Random.Range (0, noSouth.Length)];
			if (direction == Direction.North) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1f,1.5f);
				collider.offset = new Vector2 (0f, 0.5f);
				MoveNorth ();
				lastDirection = Direction.North;
			}
			if (direction == Direction.East) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1.5f,1.0f);
				collider.offset = new Vector2 (0.5f, 0f);
				MoveEast ();
				lastDirection = Direction.East;
			}
			if (direction == Direction.West) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1.5f,1.0f);
				collider.offset = new Vector2 (-0.5f, 0f);
				MoveWest ();
				lastDirection = Direction.West;
			}
		}

		else if(direction == Direction.East){
			
			direction = (Direction) noEast [Random.Range (0, noEast.Length)];
			if (direction == Direction.North) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1f,1.5f);
				collider.offset = new Vector2 (0f, 0.5f);
				MoveNorth ();
				lastDirection = Direction.North;
			}
			if (direction == Direction.South) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1.0f,1.5f);
				collider.offset = new Vector2 (0f, -0.5f);
				MoveSouth ();
				lastDirection = Direction.South;
			}
			if (direction == Direction.West) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1.5f,1.0f);
				collider.offset = new Vector2 (-0.5f, 0f);
				MoveWest ();
				lastDirection = Direction.West;
			}
		}

		else if(direction == Direction.West){

			
			direction = (Direction) noWest [Random.Range (0, noWest.Length)];
			if (direction == Direction.North) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1f,1.5f);
				collider.offset = new Vector2 (0f, 0.5f);
				MoveNorth ();
				lastDirection = Direction.North;
			}
			if (direction == Direction.East) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1.5f,1.0f);
				collider.offset = new Vector2 (0.5f, 0f);
				MoveEast ();
				lastDirection = Direction.East;
			}
			if (direction == Direction.South) {
				if (direction == lastDirection)
					DecideRoute ();
				collider.size = new Vector2 (1.0f,1.5f);
				collider.offset = new Vector2 (0f, -0.5f);
				MoveSouth ();
				lastDirection = Direction.South;
			}
		}
	}

	bool FoundObstacle(){
		return false;
	}

	void MoveSouth(){
		h = 0;
		v = -1;
		direction = Direction.South;
	}

	void MoveNorth(){
		h = 0;
		v = 1;
		direction = Direction.North;
	}

	void MoveEast(){
		h = 1;
		v = 0;
		direction = Direction.East;
	}

	void MoveWest(){
		h = -1;
		v = 0;
		direction = Direction.West;
	}

	void OnTriggerStay2D(Collider2D other){
		Debug.Log ("Collided "+direction);
		if(other.tag == "Wall"){
			if (direction == Direction.North && other.transform.position.y > transform.position.y)
				DecideRoute ();
			else if (direction == Direction.South && other.transform.position.y < transform.position.y)
				DecideRoute ();
			else if (direction == Direction.East && other.transform.position.x > transform.position.x)
				DecideRoute ();
			else if (direction == Direction.West && other.transform.position.x < transform.position.x)
				DecideRoute ();
		}
			
	}
}
