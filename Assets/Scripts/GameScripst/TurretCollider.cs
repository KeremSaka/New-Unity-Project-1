using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCollider : MonoBehaviour {
	private GameObject targets;
	private int pointer = 0;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			targets = other.gameObject;



		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			targets = null;



		}
	}


	public GameObject getTarget()
	{
		return targets;
	}
	public void delTarget()
	{
		targets = null;
	}
}