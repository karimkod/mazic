using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserOL : MonoBehaviour {

	public int traps = 10,patrollers = 3;
	public GameObject trap;
	public GameObject patroller;
	Camera cam;
	float lastX, lastY;
	private PlayerOL player;
	Animator anim;
	// Use this for initialization
	public void CmdplantTrap(){
		if(traps > 0){
			var t = Instantiate (trap,transform.position,transform.rotation);
			//NetworkServer.Spawn (t);
			traps--;
		}
	}

	public void CmdplantPatroller(){
		if(patrollers > 0){
			var t = Instantiate (patroller,transform.position,transform.rotation);
			//NetworkServer.Spawn (t);
			patrollers--;
		}
	}
	void Start () {
		player = gameObject.AddComponent<PlayerOL> ();
		player.spawnPlayer (player.maze.columns - 5, player.maze.columns - 2);
		cam = GetComponentInChildren<Camera> ();
		anim = GetComponent<Animator> ();
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
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.T)){
			CmdplantTrap ();
		}

		if(Input.GetKeyDown(KeyCode.Z)){
			CmdplantPatroller ();
		}

		Move ();
	}
		
}
