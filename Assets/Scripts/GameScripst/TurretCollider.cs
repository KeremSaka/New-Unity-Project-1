using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCollider : MonoBehaviour {
	private GameObject currentTarget;
    private int currentTargetId;
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
			if(currentTarget == null)
            {
                currentTarget = other.gameObject;
                MovePlayer mp = currentTarget.GetComponent<MovePlayer>();
                currentTargetId = mp.ID;
            }
            else
            {
                currentTarget = getNearestEnemy(currentTarget, other.gameObject);
                MovePlayer mp = currentTarget.GetComponent<MovePlayer>();
                currentTargetId = mp.ID;
            }


		}
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (currentTarget == null)
            {
                currentTarget = other.gameObject;
                MovePlayer mp = currentTarget.GetComponent<MovePlayer>();
                currentTargetId = mp.ID;
            }
            else
            {
                MovePlayer mp = other.gameObject.GetComponent<MovePlayer>();

                if (mp.ID != currentTargetId)
                {
                    currentTarget = getNearestEnemy(currentTarget, other.gameObject);
                    mp = currentTarget.GetComponent<MovePlayer>();
                    currentTargetId = mp.ID;
                }
            }


        }
    }

    private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
            MovePlayer mp = other.gameObject.GetComponent<MovePlayer>();
            if(currentTargetId == mp.ID)
            {
                currentTarget = null;
            }
        }
	}


	public GameObject getTarget()
	{
		return currentTarget;
	}
	public void delTarget()
	{
        currentTarget = null;
	}

    private GameObject getNearestEnemy(GameObject enemy1, GameObject enemy2)
    {
        float tempX = gameObject.transform.position.x - enemy1.transform.position.x;
        float tempZ = gameObject.transform.position.z - enemy1.transform.position.z;
        float distance = Mathf.Sqrt( Mathf.Pow(tempX, 2) + Mathf.Pow(tempZ, 2));
        distance = Mathf.Abs(distance);

        tempX = gameObject.transform.position.x - enemy2.transform.position.x;
        tempZ = gameObject.transform.position.z - enemy2.transform.position.z;
        float distance2 = Mathf.Sqrt(Mathf.Pow(tempX, 2) + Mathf.Pow(tempZ, 2));
        distance = Mathf.Abs(distance2);

        if (distance < distance2)
        {
            return enemy1;
        }
        else
        {
            return enemy2;
        }
    }
}