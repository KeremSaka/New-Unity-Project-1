using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerBehaviorScript : MonoBehaviour {
    public TurretCollider turretCollider;
    public GameObject target;
    public GameObject particleAttractor;
    private float damage = 1f;
    private float cooldown = 0.2f;
    private float currentTime = 0.2f;
	// Use this for initialization
	void Start () {
        turretCollider = GetComponentInChildren<TurretCollider>();
        
	}
	
	// Update is called once per frame
	void Update () {
        target = turretCollider.getTarget();
        if(target != null)
        {
            particleAttractor.transform.position = new Vector3(target.transform.position.x, target.transform.position.y +0.5f, target.transform.position.z);
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                MovePlayer mp = target.GetComponent<MovePlayer>();
                mp.dealDamage(damage);
                currentTime = cooldown;
            }
        }
        else
        {
            particleAttractor.transform.position = gameObject.transform.position;
        }
	}

   

}
