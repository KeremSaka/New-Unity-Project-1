using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed;
    private float damage = 5f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(TimeToLive(1f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed);
    }


    public float getDamage()
    {
        return damage;
    }

    IEnumerator TimeToLive(float number)
    {

        yield return new WaitForSeconds(number);
        Destroy(this.gameObject);
    }
}