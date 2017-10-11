using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject enemyPrefab;
    public Transform spawnpoint;
    public Transform destination;
	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnEnemys(5));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    IEnumerator SpawnEnemys(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnpoint.position, spawnpoint.rotation);
            MovePlayer mp = enemy.GetComponent<MovePlayer>();
            mp.setDestination(destination);
            yield return new WaitForSeconds(3.0f);
        }
    }
}
