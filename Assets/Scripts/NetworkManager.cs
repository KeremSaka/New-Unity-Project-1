using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

	GameObject mainMenu;

	public void Connect(){
		Debug.Log ("Connection wird ausgeführt...");
		PhotonNetwork.ConnectUsingSettings ("vs01");
	}

	// Use this for initialization
	void Start () {
		mainMenu = GameObject.Find ("MainMenu");
	}

	void OnConnectedToMaster(){
		Debug.Log ("Mit Master verbunden.");
		PhotonNetwork.JoinLobby ();
	}

	void OnJoinedLobby(){
		Debug.Log ("Mit der Lobby verbunden.");
		mainMenu.SetActive (false);
		PhotonNetwork.JoinRandomRoom ();
	}

	void OnPhotonRandomJoinFailed(){
		Debug.Log ("No room. Creating new room.");
		PhotonNetwork.CreateRoom("myRoom");
	}

	void OnJoinedRoom(){
		Debug.Log ("Joined room.");
		Spawn ();

	}

	void Spawn(){
		int posX = PhotonNetwork.otherPlayers.Length;
		PhotonNetwork.Instantiate ("Player", new Vector3(posX,0,0), Quaternion.identity, 0);
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (PhotonNetwork.otherPlayers.Length + "Spieler im Raum.");
	}
}
