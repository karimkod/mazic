using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Chaser : NetworkBehaviour {

	public int traps = 6;
	public GameObject trap;

    private int Traps { set { TrapsText.text = value.ToString(); traps = value; } get { return traps; } }

    public Text TrapsText; 

	Camera cam;
	private Player player;
	GameObject key;
	GameObject outage;
	public string playersText;
	public string name;
	public int id;
	float lastX, lastY;
	[SyncVar]
	public bool hasLost = false;
	[SyncVar]
	public bool playerRan = false;
	[SyncVar]
	public bool hasWon = false;
	bool winning = false;
	Animator anim;
	Rigidbody2D rg;
	GameObject[] runners;
	float h = 0;
	float v = 0;
	// Use this for initialization

	[Command]
	public void CmdplantTrap(){
		if(traps > 0){
			var t = Instantiate (trap,transform.position,transform.rotation);
            t.GetComponent<TrapScript>().Owner = this;
			NetworkServer.Spawn (t);
			Traps--;
		}
	}

	void Start () {
		player = gameObject.AddComponent<Player> ();
		player.spawnPlayer (player.maze.rows - 5, player.maze.rows - 2);
		hasWon = false;
        cam = GetComponentInChildren<Camera> ();
		player.speed = 4.2f;
		rg = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		if (!isLocalPlayer)
			cam.gameObject.SetActive (false);

		key = GameObject.FindGameObjectWithTag ("Key");
		outage = GameObject.FindGameObjectWithTag ("Outage");

		key.GetComponent<SpriteRenderer> ().enabled = false;
		outage.GetComponent<SpriteRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)// || !GameManager.startGame)// )
			return;


		if(GameManager.timeUp){

			// win screen
			hasLost = true;
			return;
		}


		if(Input.GetKeyDown(KeyCode.T)){
			CmdplantTrap ();
		}


		Move ();

		runners = GameObject.FindGameObjectsWithTag ("Runner");

		foreach(GameObject r in runners){
			float dist = Vector2.Distance (transform.position, r.gameObject.transform.position);
			Debug.Log (r.gameObject.GetComponent<Runner> ().invisibility);
			if(dist <= 2){
				
				r.gameObject.GetComponent<Runner> ().CmdDie ();

			}
			if(r.GetComponent<Runner>().invisibility == true){
				//Color c = r.GetComponent<SpriteRenderer> ().color;
				r.GetComponent<SpriteRenderer> ().enabled = false;
			}
			else{
				r.GetComponent<SpriteRenderer> ().enabled = true;
			}
			if(r.gameObject.GetComponent<Runner> ().hasWon){
				playerRan = true;
			}
			else{
				playerRan = false;
			}
			if(r.gameObject.GetComponent<Runner> ().isDead){
				winning = true;
			}
			else{
				winning = false;
			}
		}
		if (playerRan)
			CmdLose ();
		if (winning){
			Debug.Log ("Winning");
			CmdWin ();
		}
			
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
		}else{
			lastX = dir.x;
			lastY = dir.y;
			anim.SetBool ("isWalking", true);
		}
		anim.SetFloat ("xInput", dir.x);
		anim.SetFloat ("yInput", dir.y);
	}

	[Command]
	public void CmdLose(){
		hasLost = true;
	}

	[Command]
	public void CmdWin(){
		hasWon = true;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Torch")
        {
			collision.GetComponent<Animator> ().SetTrigger ("putOff");
			Destroy (collision.transform.GetChild (0).gameObject);
			//Destroy(collision.gameObject);
            //NetworkServer.UnSpawn(collision.gameObject);
        }
    }
}
