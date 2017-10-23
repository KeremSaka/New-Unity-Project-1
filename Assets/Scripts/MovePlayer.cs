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
    public bool attack = false;

    public bool wallAlive = true;//declares if the Wall is intact and hase no holes

    public int targetNR;//Id Of the wall which is tageted from this object

    public int ID;
    // Use this for initialization
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        if (Game.getMaster())
        {
            
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
        anim.SetBool("Run", true);
        anim.SetBool("Attack", false);

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

                if (!attack)
                {
                    StartCoroutine(DamageWall());
                }
            }
            if (other.gameObject.tag == "Destination")
            {
                _navMeshAgent.isStopped = true;
                anim.SetBool("Run", false);
                anim.SetBool("Attack", true);

                if (!attack)
                {
                    StartCoroutine(DamageWall());
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
        
    }

    IEnumerator Death()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);
        anim.SetBool("Death", true);
        _navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(1f);
        
        Destroy(this.gameObject);
    }

    IEnumerator DamageWall()
    {
        attack = true;
        while (wallAlive && !dying && attack)
        {
            yield return new WaitForSeconds(1.5f);
            if (game.setDamageToWall(targetNR, damage) <= 0)
            {
                wallAlive = false;
                
            }
            
        }
        while (!dying && game.TowerHealth >= 0 && attack)
        {
            yield return new WaitForSeconds(1.5f);
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
        anim.SetBool("Run", false);
        yield return new WaitForSeconds(1f);
        _navMeshAgent.isStopped = false;
        SetDestinationNavMesh();
        anim.SetBool("Run", true);
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
    public void dealDamage(float damage)
    {
        health -= damage;
      
    }
    public void Kill()
    {
        StartCoroutine(Death());
    }
}