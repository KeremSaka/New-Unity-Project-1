using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerMovement : MonoBehaviour {

	GameObject owner;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 200.0f;
		var x = 1 * Time.deltaTime * 200.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f;

		if (z != 0) {
			PhotonNetwork.Destroy(gameObject);
		}

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);
	}
}
