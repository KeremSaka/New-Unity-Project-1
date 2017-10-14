using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {

	public GameObject camera;

	// Use this for initialization
	void Start () {
		if (photonView.isMine) {
			camera.SetActive(true);
			GetComponent<PlayerMovement> ().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
