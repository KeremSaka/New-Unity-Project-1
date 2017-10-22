using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NetworkManager : MonoBehaviour {



    public Game game;

    public void Connect(){
		Debug.Log ("Connection wird ausgeführt...");
		PhotonNetwork.ConnectUsingSettings ("vs01");
	}

	// Use this for initialization
	void Start () {
		Connect ();
	}

	void OnConnectedToMaster(){
		Debug.Log ("Mit Master verbunden.");
		PhotonNetwork.JoinLobby ();
	}

	void OnJoinedLobby(){
		Debug.Log ("Mit der Lobby verbunden.");
		PhotonNetwork.JoinRandomRoom ();
	}

	void OnPhotonRandomJoinFailed(){
		Debug.Log ("No room. Creating new room.");
		PhotonNetwork.CreateRoom("myRoom");
	}

	void OnJoinedRoom(){
		Debug.Log ("Joined room.");
        //Spawn ();
        game.startGame();
	}



	void Spawn(){
		int posX = PhotonNetwork.otherPlayers.Length;

		GameObject level = PhotonNetwork.Instantiate ("FencePart", new Vector3 (posX, 5, -4), Quaternion.identity, 0);
        level.transform.parent = GameObject.Find("ImageTargetLevel").transform;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (PhotonNetwork.otherPlayers.Length + "Spieler im Raum.");
	}
}
