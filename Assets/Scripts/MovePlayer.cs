using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovePlayer : MonoBehaviour
{

    [SerializeField]
    Transform _destination;

    private Transform pastDestination = null;

    NavMeshAgent _navMeshAgent;

    public Game game;
    private float health = 10f;
    public float damage = 10f;
    public EnemyAnimationController enemyAnimation;
    private bool dying = false;
    private bool spawn = false;
    public bool wallAlive = true;
    public int targetNR;
    // Use this for initialization
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        //pastDestination = _destination;
        if (_navMeshAgent == null)
        {
            Debug.LogError("Keine NavMesh");
        }
        else
        {
            //SetDestinationNavMesh();
        }
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        if(pastDestination != _destination && !spawn)
        {
            _navMeshAgent.enabled = true;
            enemyAnimation.attack = false;
            enemyAnimation.run = true;
            pastDestination = _destination;
            SetDestinationNavMesh();
        }
      
    }

    private void SetDestinationNavMesh()
    {
        if (_destination != null)
        {

            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (!dying)
        {
            
            if (other.gameObject.tag == "Wall" && wallAlive)
            {
                _navMeshAgent.enabled = false;
                enemyAnimation.run = false;
                enemyAnimation.attack = true;
                StartCoroutine(DamageWall());
            }
            if (other.gameObject.tag == "Destination")
            {
                _navMeshAgent.enabled = false;
                enemyAnimation.run = false;
                enemyAnimation.attack = true;
                StartCoroutine(DamageWall());
            }

            if (other.gameObject.tag == "Bullet")
            {
                ProjectileMovement projectile = other.gameObject.GetComponent<ProjectileMovement>();
                decreaseHealth(projectile.getDamage());

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
        if (enemyAnimation != null)
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

    IEnumerator DamageWall()
    {
        while (wallAlive && !dying)
        {
            yield return new WaitForSeconds(2.2f);
            if (game.setDamageToWall(targetNR, damage) <= 0)
            {
                wallAlive = false;

            }
        }
        while (!dying && game.TowerHealth <= 0)
        {
            yield return new WaitForSeconds(2.2f);
            game.TowerHealth -= damage;
        }

    }
    IEnumerator Spawn()
    {
        _navMeshAgent.enabled = false;
        spawn = true;
        rotateTowards(_destination.position.x, _destination.position.z);
        yield return new WaitForSeconds(1.05f);
        spawn = false;
    }

    private void rotateTowards(float targetX, float targetZ)
    {
        float x = targetX - transform.position.x;
        float z = targetZ - transform.position.z;
        Vector3 relativPos = new Vector3(x, 0, z);//new direction of the object
        Quaternion destRotation = Quaternion.LookRotation(relativPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRotation, 100);
    }
}