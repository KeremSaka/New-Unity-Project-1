using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovePlayer : MonoBehaviour
{

    [SerializeField]
    Transform _destination;//target wich will be attacked
    

    NavMeshAgent _navMeshAgent;//Ai to navigate on the navMesh

    private Animator anim;//animator skript for aktivating animations

    public Game game;

    private float health = 10f;
    public float damage = 10f;
    //Bools inmportant to Animate the Object
    private bool dying = false;
    private bool spawn = false;
 

    public bool wallAlive = true;//declares if the Wall is intact and hase no holes

    public int targetNR;//Id Of the wall which is tageted from this object

    public int ID;
    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        if (Game.getMaster())
        {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();
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
        if (!dying && Game.getMaster())//only the Master instance needs to check for collision
        {
            
            if (other.gameObject.tag == "Wall" && wallAlive)//
            {
                _navMeshAgent.isStopped = true;//Nav Mesh agend stops in current position and will not move until started again
                anim.SetBool("Run", false);//Run animation will be stopped
                anim.SetBool("Attack", true);//Attack animation starts
                //animRun = false;
                //animAttack = true;
                StartCoroutine(DamageWall());
            }
            if (other.gameObject.tag == "Destination")
            {
                _navMeshAgent.isStopped = true;
                anim.SetBool("Run", false);
                anim.SetBool("Attack", true);
                //animRun = false;
                //animAttack = true;
                StartCoroutine(DamageWall());
            }

            if (other.gameObject.tag == "Bullet")
            {
                //Declare after new projectile types has been declared
                /*
                ProjectileMovement projectile = other.gameObject.GetComponent<ProjectileMovement>();
                decreaseHealth(projectile.getDamage());


                if (health <= 0)
                {

                    //GameData.Instance.EnemyKilled++;
                    dying = true;
                    StartCoroutine(Death());
                }*/

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
        anim.SetBool("Run", true);
        anim.SetBool("Attack", false);
    }

    IEnumerator Death()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);
        _navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(1.3f);
        
        Destroy(this.gameObject);
    }

    IEnumerator DamageWall()
    {
        while (wallAlive && !dying && Game.getMaster())
        {
            yield return new WaitForSeconds(0.75f);
            if (game.setDamageToWall(targetNR, damage) <= 0)
            {
                wallAlive = false;
                
            }
        }
        while (!dying && game.TowerHealth >= 0)
        {
            yield return new WaitForSeconds(0.75f);
            game.TowerHealth -= damage;
        }

    }
    IEnumerator Spawn()
    {
        _navMeshAgent.isStopped = true;
        spawn = true;
        rotateTowards(_destination.position.x, _destination.position.z);//Enemy starts looking in direction of the wall
        yield return new WaitForSeconds(1.8f);
        spawn = false;
        anim.SetBool("Run", true);

        _navMeshAgent.isStopped = false;
        SetDestinationNavMesh();
    }
    public void idleStart()//only there to start the Idle coroutine
    {
        StartCoroutine(Idle());
    }
    IEnumerator Idle()//a pause until the mesh is recalculated and a new destiantion can be set
    {
        yield return new WaitForSeconds(2f);
        _navMeshAgent.isStopped = false;
        SetDestinationNavMesh();
    }
    private void rotateTowards(float targetX, float targetZ)//rotate this game object towards an target X an Z position
    {
        float x = targetX - transform.position.x;
        float z = targetZ - transform.position.z;
        Vector3 relativPos = new Vector3(x, 0, z);//new direction of the object
        Quaternion destRotation = Quaternion.LookRotation(relativPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRotation, 100);
    }
    //Getter and Setter funktion

    public bool getDying()
    {
        return dying;
    }
    public float getHealth()
    {
        return health;
    }
 
}