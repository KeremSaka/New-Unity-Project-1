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
    private bool dying = false;
    private bool spawn = false;
    private bool animAttack = false;
    private bool animRun = false;
    public bool wallAlive = true;
    public int targetNR;
    public int ID;
    // Use this for initialization
    void Start()
    {
        if (game.getMaster())
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
    }

    private void Update()
    {

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
        if (!dying && game.getMaster())
        {
            
            if (other.gameObject.tag == "Wall" && wallAlive)
            {
                //_navMeshAgent.enabled = false;
                _navMeshAgent.isStopped = true;
                animRun = false;
                animAttack = true;
                StartCoroutine(DamageWall());
            }
            if (other.gameObject.tag == "Destination")
            {
                //_navMeshAgent.enabled = false;
                _navMeshAgent.isStopped = true;
                animRun = false;
                animAttack = true;
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
                    dying = true;
                    StartCoroutine(Death());
                }

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
        animAttack = false;
        animRun = true;
        //pastDestination = _destination;
        //SetDestinationNavMesh();
    }
    public void EnableNavMesh()
    {
        //_navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
    }
    IEnumerator Death()
    {
        animAttack = false;
        animRun = false;
        //_navMeshAgent.enabled = false;
        _navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1.3f);
        
        Destroy(this.gameObject);
    }

    IEnumerator DamageWall()
    {
        while (wallAlive && !dying && game.getMaster())
        {
            yield return new WaitForSeconds(1.4f);
            if (game.setDamageToWall(targetNR, damage) <= 0)
            {
                wallAlive = false;
                
            }
        }
        while (!dying && game.TowerHealth <= 0)
        {
            yield return new WaitForSeconds(1.4f);
            game.TowerHealth -= damage;
        }

    }
    IEnumerator Spawn()
    {
        //_navMeshAgent.enabled = false;
        _navMeshAgent.isStopped = true;
        spawn = true;
        rotateTowards(_destination.position.x, _destination.position.z);
        yield return new WaitForSeconds(1.8f);
        spawn = false;
        animRun = true;

        //_navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        SetDestinationNavMesh();
    }
    public void idleStart()
    {
        StartCoroutine(Idle());
    }
    IEnumerator Idle()
    {
        yield return new WaitForSeconds(2f);
        _navMeshAgent.isStopped = false;
        SetDestinationNavMesh();
    }
    private void rotateTowards(float targetX, float targetZ)
    {
        float x = targetX - transform.position.x;
        float z = targetZ - transform.position.z;
        Vector3 relativPos = new Vector3(x, 0, z);//new direction of the object
        Quaternion destRotation = Quaternion.LookRotation(relativPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRotation, 100);
    }
    public bool getAttack()
    {
        return animAttack;
    }
    public bool getRun()
    {
        return animRun;
    }
    public bool getDying()
    {
        return dying;
    }
    public float getHealth()
    {
        return health;
    }
}