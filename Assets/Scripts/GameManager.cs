using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

    float gameTime;
    public int numberOfPlayers;
    public Player player;
    public Text countDown;
    public GameObject waiting;
    public int type = 0;
    [HideInInspector]
    public GameObject pl;
    public static bool timeUp;
    [SyncVar]
    int playersNumber;

    public int id;

    [HideInInspector]
    public static bool startGame;

    [HideInInspector]
    public static int chasers = 0;
    public static int runners = 0;
    public static int lastAdded = 0;

    private float minutes;
    private float seconds;

    public GameObject runnerUI;
    public GameObject chaserUI;


    public GameObject fogOfWar;


    public Canvas HUDCanvas; 


    // Use this for initialization
    void Start()
    {

        if (numberOfPlayers == 2)
            gameTime = 90;
        else
            gameTime = 180;

        timeUp = false;

        pl = NetworkManager.singleton.client.connection.playerControllers[0].gameObject;
        if (pl.GetComponent<Runner>() == null)
        {
            chaserUI.SetActive(true);
            pl.GetComponent<Chaser>().TrapsText = chaserUI.transform.GetChild(1).GetChild(0).GetComponent<Text>();

        }
        else
        {
            runnerUI.SetActive(true);

            pl.GetComponent<Runner>().torchesText = runnerUI.transform.GetChild(2).GetChild(0).GetComponent<Text>();

            pl.GetComponent<Runner>().isShieldedIamge = runnerUI.transform.GetChild(3).GetChild(0).gameObject;

            pl.GetComponent<Runner>().keyImage = runnerUI.transform.GetChild(4).GetChild(0).gameObject;
        }

        HUDCanvas.worldCamera = pl.transform.GetChild(0).GetComponent<Camera>();
        HUDCanvas.sortingLayerName = "HUD";


        /*
       fogOfWar = GameObject.Find("FogOfWar");
        fogOfWar.GetComponent<PlayerFogOfwarSetter>().PlayerTransform = pl.gameObject.transform;    
        */

    }

    [ClientRpc]
    public void Rpcstart()
    {
        startGame = true;
    }

    [ClientRpc]
    public void RpcidAdjust()
    {
        id++;
        //id = NetworkUI.currentPlayersNumber++;
    }

    public override void OnStartServer()
    {
        playersNumber = 2;
    }
    // Update is called once per frame
    void Update()
    {


        //Debug.Log (startGame);
        if (!startGame)
        {
            waiting.SetActive(true);
            return;
        }
        waiting.SetActive(false);
        if (gameTime >= 0.0)
        {


            gameTime -= Time.deltaTime;
            minutes = Mathf.Floor(gameTime / 60);
            seconds = gameTime % 60;
            if (seconds > 60)
                seconds = 59;
            countDown.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        else
        {
            timeUp = true;
        }

        if (pl.GetComponent<Runner>() == null)
        {
            //Debug.Log ("shit");
           /* GameObject trs = chaserUI.transform.Find("Traps").gameObject;
            trs.GetComponent<Text>().text = " " + pl.GetComponent<Chaser>().traps + " traps" + pl.GetComponent<Chaser>().playersText;
            */
            GameObject[] torchs = GameObject.FindGameObjectsWithTag("Torch");

            GameObject[] signals = GameObject.FindGameObjectsWithTag("Signal");


            foreach (GameObject t in signals)
            {
                if(t.GetComponent<TrapScript>().Owner 
                    != 
                    pl.GetComponent<Chaser>())
                    t.GetComponent<SpriteRenderer>().enabled = false;
            }


            foreach (GameObject t in torchs)
            {
                t.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            }

            if (pl.GetComponent<Chaser>().hasLost)
            {
                chaserUI.transform.Find("Lost").gameObject.SetActive(true);
				return;
            }
			if (pl.GetComponent<Chaser>().hasWon)
			{
				//Debug.Log("he won!!");
				chaserUI.transform.Find("Win").gameObject.SetActive(true);
				return;
			}
        }
        else
        {

            /*
            GameObject trs = runnerUI.transform.Find("Torches").gameObject;
            trs.GetComponent<Text>().text = " " + pl.GetComponent<Runner>().torches + pl.GetComponent<Runner>().tpos;
            */
            GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
            foreach (GameObject t in traps)
            {
                t.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                t.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;

            }


            GameObject[] signals = GameObject.FindGameObjectsWithTag("Signal");


            foreach (GameObject t in signals)
            {
                   t.GetComponent<SpriteRenderer>().enabled = false;
            }


            if (pl.GetComponent<Runner>().hasWon)
            {
                // win UI
                runnerUI.transform.Find("Win").gameObject.SetActive(true);
				return;
            }
            if (pl.GetComponent<Runner>().isDead)
            {
                // win UI
                runnerUI.transform.Find("dead").gameObject.SetActive(true);
				return;
            }
        }
    }


}
