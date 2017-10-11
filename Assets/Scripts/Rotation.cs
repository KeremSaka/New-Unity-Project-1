using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
    public float rot = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rot += 5;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rot, transform.eulerAngles.z);
    }
}
