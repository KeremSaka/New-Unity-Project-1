using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBasicScript : MonoBehaviour {
	public Collider turretFieldOfView;
	private TurretCollider turretColliderScript;
	private GameObject target;
	public GameObject projectile;
	public GameObject spawnpoint;
	private float timedelay = 0.5f;
    private int TowerID;
	// Use this for initialization
	void Start () {
        TowerID = GameData.Instance.NumberOfTowers;
        GameData.Instance.NumberOfTowers++;
		turretColliderScript =turretFieldOfView.GetComponent<TurretCollider>();
	}

	// Update is called once per frame
	void Update () {

		target = turretColliderScript.getTarget();
		if (target != null)
		{
			rotateTowards(target.transform.position.x, target.transform.position.y, target.transform.position.z);
			shoot();
		}
	}

	private void rotateTowards(float targetX, float targetY,float targetZ)
	{
		float x = targetX - transform.position.x;
		float y = targetY - transform.position.y;
		float z = targetZ - transform.position.z;
		Vector3 relativPos = new Vector3(x, y, z);//new direction of the object
		Quaternion destRotation = Quaternion.LookRotation(relativPos);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, destRotation, 10);
	}

	private void shoot()
	{

		timedelay -= Time.deltaTime;
		if (timedelay  <= 0)
		{
			Instantiate(projectile, spawnpoint.transform.position, gameObject.transform.rotation);
			timedelay = 0.5f;
		}
	}
}
