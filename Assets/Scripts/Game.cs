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
    public float spawntime;
    public int MaxEnemyNumber = 1;
    private bool master = false;
    public NavMeshSurface navMesh;

    public _NetworkManager networkManager;
	// Use this for initialization
	void Start () {
        GameData.Instance.MaxEnemyNumber = (GameData.Instance.LevelNumber + 1) * 10;

        Enemys = new MovePlayer[MaxEnemyNumber];
    
        //StartCoroutine(SpawnEnemys(MaxEnemyNumber));
        WallHealth = new float[Walls.Length];
        for(int i = 0; i< Walls.Length; i++)
        {
            WallHealth[i] = 250f;
        }
        //navMesh.BuildNavMesh();
	}
	
	// Update is called once per frame
	void Update () {
       
    }
    public void startGame() { 
		if (PhotonNetwork.otherPlayers.Length == 0)
		{
			master = true;
			SpawnFences();
		}
        navMesh.BuildNavMesh();
        StartCoroutine(SpawnEnemys(MaxEnemyNumber));
    }
    private void SpawnFences()
    {
        for (int i = 0; i< Walls.Length; i++) {
            Quaternion rotation = Quaternion.identity;
            if(i ==1 || i == 3)
            {
                rotation.eulerAngles = new Vector3(0,90,0);
            }
            Walls[i] = PhotonNetwork.Instantiate("FencePart", Walls[i].transform.position, rotation, 0);
        }
    }

    IEnumerator SpawnEnemys(int number)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < number; i++)
        {
            
            int temp = Random.Range(0, spawnpoint.Length - 1);
            GameObject enemy = Instantiate(enemyPrefab, spawnpoint[temp], false);
            //enemy.transform.localScale = new Vector3(3f, 3f, 3f);
            MovePlayer mp = enemy.GetComponent<MovePlayer>();
            mp.ID = i;
            Enemys[i] = mp;
            int target = NearestWall(spawnpoint[temp]);
            
            mp.game = this;
            mp.targetNR = target;
            mp.setDestination(Walls[target].transform);
            yield return new WaitForSeconds(spawntime);

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
		if(WallHealth[target] <= 0 && Walls[target]!=null && master)
        {
            PhotonNetwork.Destroy(Walls[target]);
            //Destroy(Walls[target].gameObject);
            Walls[target] = null;
           
            setTowerDestination();
        }
        return WallHealth[target];
    }


    private void setTowerDestination()
    {
        navMesh.RemoveData();
        
        
        for (int i = 0; i< Enemys.Length; i++)
        {
           
            Enemys[i].wallAlive = false;
            Enemys[i].setDestination(Tower);
            Enemys[i].idleStart();
        }
        navMesh.BuildNavMesh();
    }
}
