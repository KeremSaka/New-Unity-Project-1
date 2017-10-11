using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovePlayer : MonoBehaviour {

	[SerializeField]
	Transform _destination;

	NavMeshAgent _navMeshAgent;

    private float health = 10f;

	// Use this for initialization
	void Start () {

		_navMeshAgent = this.GetComponent<NavMeshAgent> ();

		if (_navMeshAgent == null) {
			Debug.LogError ("Keine NavMesh");
		} else {
			SetDestination ();
		}
        GameData.Instance.NumberOfEnemy++;
	}
	
	private void SetDestination(){

		if (_destination != null) {
			Vector3 targetVector = _destination.transform.position;
			_navMeshAgent.SetDestination (targetVector);
		}
	}

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Destination") {
            GameData.Instance.EnemyReachedDestination++;
            GameData.Instance.NumberOfEnemy--;
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "Bullet")
        {
            ProjectileMovement projectile =  other.gameObject.GetComponent<ProjectileMovement>();
            decreaseHealth(projectile.getDamage());
            Debug.Log(health);
            Destroy(other.gameObject);
            if (health <= 0)
            {
                GameData.Instance.EnemyKilled++;
                GameData.Instance.NumberOfEnemy--;
                
                Destroy(this.gameObject);
            }
            
        }

    }

    public void decreaseHealth(float damage)
    {
        health -= damage;

    }

    public void setDestination(Transform destination)
    {
        _destination = destination;
    }
}
