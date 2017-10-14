using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Game : MonoBehaviour {


    public Transform[] spawnpoint;
    public Transform Tower;

    public GameObject enemyPrefab;
    public GameObject[] Walls;

    public MovePlayer[] Enemys;

    public float[] WallHealth;
    public float TowerHealth = 100;

    public int MaxEnemyNumber = 1;

    public NavMeshSurface meshSurface;

	// Use this for initialization
	void Start () {
        GameData.Instance.MaxEnemyNumber = (GameData.Instance.LevelNumber + 1) * 10;
        meshSurface.BuildNavMesh();
        Enemys = new MovePlayer[MaxEnemyNumber];
    
        StartCoroutine(SpawnEnemys(MaxEnemyNumber));
        WallHealth = new float[Walls.Length];
        for(int i = 0; i< Walls.Length; i++)
        {
            WallHealth[i] = 500f;
        }
	}
	
	// Update is called once per frame
	void Update () {
        meshSurface.BuildNavMesh();
    }

    IEnumerator SpawnEnemys(int number)
    {
        for (int i = 0; i < number; i++)
        {
            meshSurface.BuildNavMesh();
            int temp = Random.Range(0, spawnpoint.Length - 1);
            GameObject enemy = Instantiate(enemyPrefab, spawnpoint[temp], false);
            //enemy.transform.localScale = new Vector3(3f, 3f, 3f);
            MovePlayer mp = enemy.GetComponent<MovePlayer>();
            mp.ID = i;
            Enemys[i] = mp;
            int target = NearestWall(spawnpoint[temp]);
            
            mp.game = this;
            mp.targetNR = target;
            mp.meshS = meshSurface;
            mp.setDestination(Walls[target].transform);
            yield return new WaitForSeconds(2.0f);

        }
        
    }
    public int NearestWall(Transform tp)
    {

        int nearest = 0;
        float shortestDistance = 200f;
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
        if(WallHealth[target] <= 0 && Walls[target]!=null)
        {
            
            Destroy(Walls[target].gameObject);
            Walls[target] = null;
            if(target == 0)
            {
                int temp = Walls.Length - 1;
                if (Walls[temp] != null)
                {
                    Destroy(Walls[temp].gameObject);
                    Walls[temp] = null;
                }
            }
            else
            {
                if (Walls[target-1] != null)
                {
                    Destroy(Walls[target - 1].gameObject);
                    Walls[target - 1] = null;
                }
            }
            if (target == Walls.Length - 1)
            {
                if (Walls[0] != null)
                {
                    Destroy(Walls[0].gameObject);
                    Walls[0] = null;
                }
            }
            else
            {
                if (Walls[target+1] != null)
                {
                    Destroy(Walls[target + 1].gameObject);
                    Walls[target + 1] = null;
                }
            }
            meshSurface.BuildNavMesh();
            setTowerDestination();
        }
        return WallHealth[target];
    }


    private void setTowerDestination()
    {
        for (int i = 0; i< Enemys.Length; i++)
        {
            Enemys[i].wallAlive = false;
            Enemys[i].setDestination(Tower);
            Enemys[i].EnableNavMesh();
        }
        
    }
}
