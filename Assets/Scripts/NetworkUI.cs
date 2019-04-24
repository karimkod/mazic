using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Character{
	Chaser,
	Runner
};

public class NetworkUI : NetworkManager {
	[HideInInspector]
	public Character character;
	[HideInInspector]
	public int chosenCharacter = 0;
	int playersNumber = 2;
	[HideInInspector]
	public static int currentPlayersNumber = 0;
	bool continueGame = false;
	int selectedChar;
	GameManager gm;
	GameObject player;
	public int playerId;

	NetworkUI nm;

	private int connectionType; //if 0 host if 1 client.

	[Header("UI Game Objects")]
	public GameObject mainMenu;
	public GameObject charMenu;
	public GameObject playersMenu;
	public GameObject continueMenu;

	[Header("Game Characters")]
	public GameObject chaserPrefab;
	public GameObject runnerPrefab;

	public void CreateGame () {
		
		mainMenu.SetActive (true);
		charMenu.SetActive (false);
		nm = GetComponent<NetworkUI> ();
		SetPort ();
		mainMenu.SetActive (false);
		charMenu.SetActive (true);
		//NetworkManager.singleton.StartHost ();
		connectionType = 0;
	}

	public void JoinGame(){

		mainMenu.SetActive (true);
		charMenu.SetActive (false);
		SetIPAdress();
		SetPort();
		mainMenu.SetActive (false);
		charMenu.SetActive (true);
		connectionType = 1;
		//NetworkManager.singleton.StartClient();

	}

	void SetIPAdress(){
		
		string ipAdress = GameObject.Find ("AdressField").transform.Find ("Text").GetComponent<Text>().text;
		NetworkUI.singleton.networkAddress = ipAdress;

	}

	void SetPort(){
		
		NetworkUI.singleton.networkPort = 7777;
	}

	public void ExitGame(){
        Application.Quit();
	}

	public void chooseChaser(){
		
		//nm.playerPrefab = prefabs;
		character = Character.Chaser;
		chosenCharacter = 0;
		currentPlayersNumber++;
		charMenu.SetActive (false);
		if (connectionType == 0)
			playersMenu.SetActive (true);
		else if (connectionType == 1)
			NetworkUI.singleton.StartClient ();
		//waiting.SetActive (true);
		
	}

	public void chooseRunner(){
		
		//nm.playerPrefab = prefabs;
		character = Character.Runner;
		currentPlayersNumber++;
		chosenCharacter = 1;
		charMenu.SetActive (false);
		if (connectionType == 0)
			playersMenu.SetActive (true);
		else if (connectionType == 1)
			NetworkUI.singleton.StartClient ();
		//waiting.SetActive (true);

	}

	public class NetworkMessage : MessageBase {
		public int chosenChar;

	}

	public void twoPlayers(){
		
		playersNumber = 2;
		//if(lobbyFull)
		NetworkUI.singleton.StartHost ();

	}
		
	public void fourPLayers(){
		
		playersNumber = 4;
		NetworkUI.singleton.StartHost ();
	}

	public void startGame(){
		
		if (connectionType == 0)
			NetworkUI.singleton.StartHost ();
		else if (connectionType == 1)
			NetworkUI.singleton.StartClient ();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
		
		NetworkMessage message = extraMessageReader.ReadMessage< NetworkMessage>();
		selectedChar = message.chosenChar;
		playerId = playerControllerId;
		Debug.Log("server add with message "+ selectedChar);

		if (selectedChar == 0 ) {
			if(GameManager.chasers < playersNumber / 2){

				player = Instantiate(chaserPrefab) as GameObject;
				NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

			}
			else{

				player = Instantiate(runnerPrefab) as GameObject;
				NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

			}
		}

		if (selectedChar == 1) {
			if(GameManager.runners < playersNumber / 2){
				
				player = Instantiate(runnerPrefab) as GameObject;
				NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

			}
			else{
				
				Debug.Log (chaserPrefab.GetComponent<Chaser> ().id);
				player = Instantiate(chaserPrefab) as GameObject;
				player.GetComponent<Chaser> ().id = playerControllerId;
				NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

			}
		}
	}

	public override void OnClientConnect(NetworkConnection conn) {
		
		NetworkMessage test = new NetworkMessage();

		test.chosenChar = chosenCharacter;
		currentPlayersNumber = NetworkServer.connections.Count;
		ClientScene.AddPlayer(conn, 0, test);

	}
		
	public override void OnClientSceneChanged(NetworkConnection conn) {


		Debug.Log ("player id " + playerId);

		if (selectedChar == 0)
			GameManager.chasers++;
		else
			GameManager.runners++;
		//if(lobbyFull)
	}

	void Update(){


		//Debug.Log ("id current" + GameManager.id);
		if(playersNumber == NetworkServer.connections.Count){
			GameObject.Find ("GameManager").GetComponent<GameManager> ().Rpcstart ();
		}
	}
		
}
