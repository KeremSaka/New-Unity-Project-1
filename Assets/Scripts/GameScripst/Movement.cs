using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	// Use this for initialization
	void Start () {
   
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    
    //2d rotation from y Axis from an object, direction to X and Z positon of a new Target 
    public void rotateTowards(float targetX, float targetZ)
    {
        float x = targetX - transform.position.x;
        float z = targetZ - transform.position.z;
        Vector3 relativPos = new Vector3(x, 0, z);//new direction of the object
        Quaternion destRotation = Quaternion.LookRotation(relativPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRotation, 10);
    }
}
