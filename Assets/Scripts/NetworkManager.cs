using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

	private bool createdLevel = true;

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
		createdLevel = false;
		PhotonNetwork.CreateRoom("myRoom");
	}

	void OnJoinedRoom(){
		Debug.Log ("Joined room.");
		Spawn ();

	}

	void Spawn(){
		int posX = PhotonNetwork.otherPlayers.Length;
		if (!createdLevel) {
			GameObject level = PhotonNetwork.Instantiate ("Level", new Vector3 (posX, 0, 0), Quaternion.identity, 0);
			Debug.Log (level.name);
			level.transform.parent = GameObject.Find ("ImageTarget").transform;
			createdLevel = true;
		}
		GameObject player = PhotonNetwork.Instantiate ("Player", new Vector3(posX,1,0), Quaternion.identity, 0);
		Debug.Log(player.name);
		player.transform.parent = GameObject.Find ("ImageTarget").transform;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (PhotonNetwork.otherPlayers.Length + "Spieler im Raum.");
	}
}
