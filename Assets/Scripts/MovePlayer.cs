using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovePlayer : MonoBehaviour
{

    [SerializeField]
    Transform _destination;

    private Transform pastDestination;

    NavMeshAgent _navMeshAgent;

    public Game game;
    private float health = 10f;
    public float damage = 10f;
    public EnemyAnimationController enemyAnimation;
    private bool dying = false;
    public bool wallAlive = true;
    public int targetNR;
    // Use this for initialization
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        pastDestination = _destination;
        if (_navMeshAgent == null)
        {
            Debug.LogError("Keine NavMesh");
        }
        else
        {
            SetDestinationNavMesh();
        }
        //StartCoroutine(Spawn());
    }

    private void Update()
    {
        if(pastDestination != _destination)
        {
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
        if (!dying)
        {
            if (other.gameObject.tag == "Wall" && !enemyAnimation.attack)
            {
                _navMeshAgent.enabled = false;
                enemyAnimation.attack = true;
                StartCoroutine(DamageWall());
            }
            if (other.gameObject.tag == "Destination")
            {
                _navMeshAgent.enabled = false;
                enemyAnimation.attack = true;
                StartCoroutine(DamageWall());
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
        yield return new WaitForSeconds(1.05f);
        _navMeshAgent.enabled = true;
    }
}