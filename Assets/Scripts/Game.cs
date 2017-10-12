using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject enemyPrefab;
    public Transform[] spawnpoint;
    public Transform destination;
    public Transform Tower;
    public GameObject[] Walls;
    private MovePlayer[] Enemys;
    public float[] WallHealth;
    public float TowerHealth = 100;
	// Use this for initialization
	void Start () {
        GameData.Instance.MaxEnemyNumber = (GameData.Instance.LevelNumber + 1) * 10;
        StartCoroutine(SpawnEnemys(GameData.Instance.MaxEnemyNumber));
        Enemys = new MovePlayer[GameData.Instance.MaxEnemyNumber];
        WallHealth = new float[Walls.Length];
        for(int i = 0; i< Walls.Length; i++)
        {
            WallHealth[i] = 100f;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnEnemys(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnpoint[Random.Range(0, spawnpoint.Length - 1)], false);
            MovePlayer mp = enemy.GetComponent<MovePlayer>();
            Enemys[i] = mp;
            int target = NearestWall(enemy.transform);
            mp.setDestination(Walls[target].transform);
            mp.game = this;
            mp.targetNR = target;
            yield return new WaitForSeconds(3.0f);
        }
    }
    public int NearestWall(Transform tp)
    {
        int nearest = 0;
        float shortestDistance = 200000;
        for (int i = 0; i < Walls.Length - 1; i++)
        {
            GameObject temp =Walls[i];
            float tempX = temp.transform.position.x - tp.position.x;
            float tempZ = temp.transform.position.z - tp.position.z;
            tempX = Mathf.Pow(tempX, 2);
            tempZ = Mathf.Pow(tempZ, 2);
            float distance = Mathf.Sqrt(tempX + tempZ);
            distance = Mathf.Abs(distance);
            if (shortestDistance > distance)
            {
                shortestDistance = distance;
                nearest = i;
            }
        }
        return nearest;
    }
    private void OnGUI()
    {
        GUI.Box(new Rect(Screen.width - 150, 0, 150, 50),"Enemys killed: " +  GameData.Instance.EnemyKilled);
        GUI.Box(new Rect(Screen.width - 150, 20, 150, 50), "Enemys missed: " + GameData.Instance.EnemyReachedDestination);
        GUI.Box(new Rect(Screen.width - 150, 40, 150, 50), "Max Enemy Number: " + GameData.Instance.MaxEnemyNumber);
        GUI.Box(new Rect(Screen.width - 150, 60, 150, 50), "Number Of Tower: " + GameData.Instance.NumberOfTowers);
        GUI.Box(new Rect(Screen.width - 150, 80, 150, 50), "Current Level: " + (GameData.Instance.LevelNumber +1));
    }

    public float setDamageToWall(int target, float damage)
    {
        WallHealth[target] -= damage;
        if(WallHealth[target] <= 0)
        {
            setTowerDestination();
        }
        return WallHealth[target];
    }


    private void setTowerDestination()
    {
        for(int i = 0; i< Enemys.Length; i++)
        {
            Enemys[i].setDestination(Tower);
        }
    }
}
