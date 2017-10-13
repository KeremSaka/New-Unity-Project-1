using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour {
    private Animator anim;
    private int damageHash = Animator.StringToHash("Damage");
    private int deathHash = Animator.StringToHash("Death");
    private int attackHash = Animator.StringToHash("Attack");
    private int runHash = Animator.StringToHash("Run");
    public bool getDamage = false;
    public bool isDead = false;
    public bool attack = false;
    public bool run = false;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (run && !attack)
        {
            anim.SetTrigger(runHash);
            if (isDead)
            {
                anim.SetTrigger(deathHash);
            }
        }
      

        if (getDamage)
        {
            anim.SetTrigger(damageHash);
            getDamage = false;
            if (isDead)
            {
                anim.SetTrigger(deathHash);
            }

        }
        if (attack)
        {
            anim.SetTrigger(attackHash);

        }
        
    }


}
