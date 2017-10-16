using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelletonAnimation : MonoBehaviour {
    private Animator anim;
   
    private int deathHash = Animator.StringToHash("Death");
    private int attackHash = Animator.StringToHash("Attack");
    private int runHash = Animator.StringToHash("Run");
    private int chargeHash = Animator.StringToHash("Charge");
    private int wallHash = Animator.StringToHash("Wallreached");
    private float health;
    public MovePlayer enemy;

    // Use this for initialization
    void Start () {
        health = enemy.getHealth() /2;
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (Game.getMaster()) { 
            if (enemy.getRun())
            {
                anim.SetBool(runHash, true);
               if(enemy.getHealth() <= health)
                {
                    anim.SetTrigger(chargeHash);
                
                }
            }
            else
            {
                anim.SetBool(runHash, false);
            }
            if (enemy.getAttack())
            {
                anim.SetBool(attackHash, true);
            }
            else
            {
                anim.SetBool(attackHash, false);
            }

            if (enemy.getDying())
            {
                anim.SetTrigger(deathHash);
            }
        }*/
    }
}
