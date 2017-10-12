using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovePlayer : MonoBehaviour {

	[SerializeField]
	Transform _destination;

	NavMeshAgent _navMeshAgent;

    private float health = 10f;
    public EnemyAnimationController enemyAnimation;
    private bool dying = false;
	// Use this for initialization
	void Start () {

		_navMeshAgent = this.GetComponent<NavMeshAgent> ();

		if (_navMeshAgent == null) {
			Debug.LogError ("Keine NavMesh");
		} else {
			SetDestination ();
		}

	}
	
	private void SetDestination(){
        if (!dying)
        {
            if (_destination != null)
            {
                Vector3 targetVector = _destination.transform.position;
                _navMeshAgent.SetDestination(targetVector);
            }
        }
	}

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other){
        if (!dying)
        {
            if(other.gameObject.tag == "Wall")
            {
                enemyAnimation.attack = true;

            }
            if (other.gameObject.tag == "Destination")
            {
                GameData.Instance.EnemyReachedDestination++;

                Destroy(this.gameObject);
            }

            if (other.gameObject.tag == "Bullet")
            {
                ProjectileMovement projectile = other.gameObject.GetComponent<ProjectileMovement>();
                decreaseHealth(projectile.getDamage());
                Debug.Log(health);
                Destroy(other.gameObject);
                if (health <= 0)
                {
                    GameData.Instance.EnemyKilled++;

                    enemyAnimation.isDead = true;
                    StartCoroutine(Death());
                }

            }
        }

    }

    public void decreaseHealth(float damage)
    {
        health -= damage;
        if(enemyAnimation != null)
        {
            enemyAnimation.getDamage = true;
        }
    }

    public void setDestination(Transform destination)
    {
        _destination = destination;
    }

    IEnumerator Death()
    {
        _navMeshAgent.enabled = false;
        yield return new WaitForSeconds(2.5f);

        Destroy(this.gameObject);
    }
}
