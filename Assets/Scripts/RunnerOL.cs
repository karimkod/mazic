using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class RunnerOL : MonoBehaviour {


	// skills
	public int torches = 6;
	public GameObject torch;
	float lastX, lastY;
	private PlayerOL player;
	bool isWalking;
	Animator anim;
	Camera cam;
	Rigidbody2D rg;
	//public float health;
//	[Command]


	public void CmdplantTorch(){
		if(torches > 0){
			var t = Instantiate (torch,transform.position,transform.rotation);
			//NetworkServer.Spawn (t);
			torches--;
		}
	}

	public void fallOnTrap(){
		// send signal
		Debug.Log("oh shit i fell on a trap");
	}
	// Use this for initialization
	void Start () {
		
		player = gameObject.AddComponent<PlayerOL> ();
		player.spawnPlayer (1, 5);
		cam = GetComponentInChildren<Camera> ();
		anim = GetComponent<Animator> ();
		rg = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.R)){
			anim.SetTrigger ("plantTorch");
			CmdplantTorch ();
		}
		Move ();

	}

	void Move(){

		Vector3 HorMovement = Vector3.right * player.speed * Time.deltaTime * Input.GetAxis ("Horizontal");
		Vector3 VerMovement = Vector3.up * player.speed * Time.deltaTime * Input.GetAxis ("Vertical");

		Vector3 heading = Vector3.Normalize (HorMovement + VerMovement);

		transform.position += HorMovement;
		transform.position += VerMovement;

		Animate (heading);
	}

	void Animate(Vector3 dir){



		if (dir.x == 0f && dir.y == 0f){
			anim.SetFloat ("lastX", lastX);
			anim.SetFloat ("lastY", lastY);
			anim.SetBool ("isWalking", false);

			anim.SetFloat ("torchX", lastX);
			anim.SetFloat ("torchY", lastY);


		}else{
			lastX = dir.x;
			lastY = dir.y;

			anim.SetFloat ("torchX", dir.x);
			anim.SetFloat ("torchY", dir.y);

			anim.SetBool ("isWalking", true);
		}


		anim.SetFloat ("xInput", dir.x);
		anim.SetFloat ("yInput", dir.y);





	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Outage"){
			Debug.Log ("Player just got out!");
		}

		if(other.tag == "Trap"){
			fallOnTrap ();
		}
		/*if(other.tag == "Patroller"){
			Debug.Log ("got hit by patroller");
		}*/
	}

	void FixedUpdate(){
		
	}
		
}
