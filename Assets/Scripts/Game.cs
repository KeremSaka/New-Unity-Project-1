using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject enemyPrefab;
    public Transform spawnpoint;
    public Transform destination;
	// Use this for initialization
	void Start () {
        GameData.Instance.MaxEnemyNumber = (GameData.Instance.LevelNumber + 1) * 10;
        StartCoroutine(SpawnEnemys(GameData.Instance.MaxEnemyNumber));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnEnemys(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnpoint, false);
            MovePlayer mp = enemy.GetComponent<MovePlayer>();
            mp.setDestination(destination);
            yield return new WaitForSeconds(3.0f);
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(Screen.width - 150, 0, 150, 50),"Enemys killed: " +  GameData.Instance.EnemyKilled);
        GUI.Box(new Rect(Screen.width - 150, 20, 150, 50), "Enemys missed: " + GameData.Instance.EnemyReachedDestination);
        GUI.Box(new Rect(Screen.width - 150, 40, 150, 50), "Max Enemy Number: " + GameData.Instance.MaxEnemyNumber);
        GUI.Box(new Rect(Screen.width - 150, 60, 150, 50), "Number Of Tower: " + GameData.Instance.NumberOfTowers);
        GUI.Box(new Rect(Screen.width - 150, 80, 150, 50), "Current Level: " + (GameData.Instance.LevelNumber +1));
    }
}
