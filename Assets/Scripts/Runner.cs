using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Runner : NetworkBehaviour {


	// skills
	public int torches = 6;
    private int Torches { set { torchesText.text = value.ToString(); torches = value; } get { return torches; } }

    public Text torchesText; 


	public string tpos = "";
	public string mazePos = "";
	public GameObject torch;
	float lastX, lastY;
	private Player player;
	public SpriteRenderer sr;
	public bool hasKey = false;
    public bool HasKey { set { keyImage.SetActive(value); hasKey = value; } get { return hasKey; }}

    public GameObject keyImage;
    
	[SyncVar]
	public bool isShielded = false;
    public bool IsSHielded { set { isShieldedIamge.SetActive(value); isShielded = value; } get { return isShielded; } }

    public GameObject isShieldedIamge;

	[SyncVar]
	public bool invisibility = false;
	[SyncVar]
	public bool hasLost =  false;
	public bool hasInvisibility = true;
	bool canMove = true;
	bool isWalking;
	[SyncVar]
	public bool isDead = false;
	[SyncVar]
	public bool hasWon = false;
	Animator anim;
	Camera cam;
	Rigidbody2D rg;

	public string name;
	public int id;

    public GameObject signalPrefab;


	string getPosition(string text){

		int rows = player.maze.rows;
		int columns = player.maze.columns;

		float posX = transform.position.x ;
		float posY = transform.position.y ;

		if(posX <= rows / 3 && posY <= columns / 3)
			text += " sw";
		else if(posX <= (rows / 3) * 2 && posY <= columns / 3)
			text += " wc";
		else if(posX <= rows / 3 && posY <= (columns / 3)*2)
			text += " sc";
		else if(posX <= (rows / 3) * 2  && posY <= columns * 2 )
			text += " cc";
		else if(posX <= rows && posY <= columns / 3 )
			text += " nw";
		else if(posX <= rows / 3  && posY <= columns)
			text += " se";
		else if(posX <= rows / 3 * 2 && posY <= columns)
			text += " sc";
		else if(posX <= rows && posY <= columns)
			text += " ne";
		else if(posX <= rows && posY <= columns / 3 * 2)
			text += " nc";

		return text;
	}
	[Command]
	public void CmdplantTorch(){
		if(torches > 0){

			var t = Instantiate (torch,transform.position,transform.rotation);
			anim.SetTrigger ("plantTorch");
			StartCoroutine("waitForTorch");
			tpos = getPosition (tpos);
			NetworkServer.Spawn (t);
			Torches = Torches - 1;
		}
	}

	[Command]
	public void CmdfallOnTrap(){
		if (!IsSHielded){
			StartCoroutine ("slowDown");
			mazePos = getPosition (mazePos);
            //mazePos = getPosition (mazePos);
            GameObject signal = Instantiate(signalPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(signal);
		}
		else
			IsSHielded = false;
		// send signal
	}
    // Use this for initialization


    [SerializeField]
    private  GameObject fogOfWarPack; 
	void Start () {

		player = gameObject.AddComponent<Player> ();
		player.spawnPlayer (1, 5);
		player.speed = 3.5f;
		canMove = true;
		hasInvisibility = true;
		hasLost = false;
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		rg = GetComponent<Rigidbody2D> ();

        cam = GetComponentInChildren<Camera>();
        if (!isLocalPlayer)
            cam.gameObject.SetActive(false);

        fogOfWarPack = GameObject.Find("FogOfWarPack");

      //   torchesText = GameObject.FindGameObjectWithTag("RunnerUI").transform.GetChild(0).GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
		if (!isLocalPlayer )//|| !GameManager.startGame || isDead || hasWon) // 
			return;
		if(Input.GetKeyDown(KeyCode.R)){
			CmdplantTorch ();
		}

		if (Input.GetKeyDown (KeyCode.T)) { 
			if (hasInvisibility) {
				CmdBeInvisible ();
				StartCoroutine ("setInvisible");
				hasInvisibility = false;
			}
		}


		Move ();

		if(GameManager.timeUp){
			CmdDie ();
		}
			
	}


	[Command]
	public void CmdhasKey(){
		HasKey = true;
	}

	void OnTriggerEnter2D(Collider2D other){
		
		var collision = other.gameObject;

		if(collision.tag == "Outage"){
			var exit = collision.GetComponent<Exit>();
			if(exit.isOpened){
				Debug.Log ("Player just got out");
				CmdWin ();
				// set win anim
			}
		}

		if(collision.tag == "Key"){
			HasKey = true;
			collision.GetComponent<Animator> ().SetTrigger ("Taken");
			Destroy (collision, 1f);
		}

		if(collision.tag == "Shield"){
			collision.GetComponent<Animator> ().SetTrigger ("Taken");
			Destroy (collision, 1f);
			IsSHielded = true;
		}


    if (collision.tag == "Trap")
    {
        Destroy(collision.gameObject);
        NetworkServer.UnSpawn(collision.gameObject);
        CmdfallOnTrap();
    }
        if (collision.tag == "Invisibility"){
			Destroy (collision, 0.5f);
			invisibility = true;
		}
	}

	void Move(){

		if (!canMove)
			return;
		
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

	public override void OnStartLocalPlayer ()
	{
		//cam.GetComponent<CameraFollow> ().setFollowed (gameObject.transform);
	}

	[Command]
	public void CmdDie(){

		isDead = true;
		// die animation
		/*sr.enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;*/
		Destroy (gameObject,3.5f);
		//NetworkServer.UnSpawn (gameObject);
		
	}
	[Command]
	public void CmdWin(){
		hasWon = true;
	}
	[Command]
	public void CmdLose(){
		hasLost = true;
	}
	[Command]
	void CmdBeInvisible(){
		
		StartCoroutine("setInvisible");
	}

	IEnumerator slowDown(){
		player.speed = player.speed / 1.5f;
		yield return new WaitForSeconds (2f);
		player.speed = player.speed * 1.5f;
	}

	IEnumerator setInvisible(){
		invisibility = true;
		Color c = sr.color;
		c.a = 0.3f;
		sr.color = c;
		yield return new WaitForSeconds (5f);
		invisibility = false;
		//c = sr.color;
		c.a = 1f;
		sr.color = c;
		hasInvisibility = false;
	}

	IEnumerator waitForTorch(){
		canMove = false;
		yield return new WaitForSeconds (1.7f);
		canMove = true;
	}

}
