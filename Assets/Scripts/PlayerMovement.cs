using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Animator animator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 250.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f;

		if (z == 0f) {
			animator.SetBool ("Run",false);
		} else {
			animator.SetBool ("Run",true);
		}

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);
	}
}
