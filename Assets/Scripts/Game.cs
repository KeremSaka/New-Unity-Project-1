using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Game : MonoBehaviour {


    public Transform[] spawnpoint;
    public Transform Tower;

    public GameObject enemyPrefab;
    public GameObject[] Walls;
    public GameObject fencePrefab;

    public MovePlayer[] Enemys;

    public float[] WallHealth;
    public float TowerHealth = 1000;
    public float spawntime;
    public int MaxEnemyNumber;
    private static bool master = false;
    public NavMeshSurface navMesh;

    public _NetworkManager networkManager;
	// Use this for initialization
	void Start () {

        Enemys = new MovePlayer[MaxEnemyNumber];
    
        //StartCoroutine(SpawnEnemys(MaxEnemyNumber));
        WallHealth = new float[Walls.Length];
        for(int i = 0; i< Walls.Length; i++)
        {
            WallHealth[i] = 250f;
        }

	}
	
	// Update is called once per frame
	void Update () {
       
    }

    public void startGame() {
        navMesh.BuildNavMesh();
        if (PhotonNetwork.otherPlayers.Length == 0)
        {
            master = true;
            SpawnFences();

            StartCoroutine(SpawnEnemys(MaxEnemyNumber));
        }

    }
    private void SpawnFences()
    {
        for (int i = 0; i< Walls.Length; i++) {
            Quaternion rotation = Quaternion.identity;
            Vector3 temp = Walls[i].transform.position;
            if (i ==0 || i == 3)
            {
                rotation.eulerAngles = new Vector3(0,90,0);
           
            }
    
            
            Walls[i] = PhotonNetwork.Instantiate("Fence", temp, rotation, 0);
            //Walls[i].transform.localPosition = temp;


            Walls[i].transform.parent = GameObject.Find("ImageTargetLevel").transform;
        }
        navMesh.BuildNavMesh();
    }

    IEnumerator SpawnEnemys(int number)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < number; i++)
        {
            
            int temp = Random.Range(0, spawnpoint.Length - 1);
            GameObject enemy = PhotonNetwork.Instantiate("SkelletonEnemy 1", spawnpoint[temp].position, Quaternion.identity, 0);
            enemy.transform.parent = GameObject.Find("ImageTargetLevel").transform;
            //Setting parameters of the Enemy gameObject
            MovePlayer mp = enemy.GetComponent<MovePlayer>();
            
            mp.ID = i;
            
            int target = NearestWall(spawnpoint[temp]);//id of the Wall which will be targeted
            
            mp.game = this;
            mp.targetNR = target;
            Debug.Log(i + "Loop");
            mp.setDestination(Walls[target].transform);
            Debug.Log(i + "Loop after SetDestination");
            Enemys[i] = mp;
            yield return new WaitForSeconds(spawntime);

        }
        
    }
    public int NearestWall(Transform tp)//calculates the neerest wall for the gammeObject tp and returns the id of that wall
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
        /*
        GUI.Box(new Rect(Screen.width - 150, 0, 150, 50),"Enemys killed: " +  GameData.Instance.EnemyKilled);
        GUI.Box(new Rect(Screen.width - 150, 20, 150, 50), "Enemys missed: " + GameData.Instance.EnemyReachedDestination);
        GUI.Box(new Rect(Screen.width - 150, 40, 150, 50), "Max Enemy Number: " + GameData.Instance.MaxEnemyNumber);
        GUI.Box(new Rect(Screen.width - 150, 60, 150, 50), "Number Of Tower: " + GameData.Instance.NumberOfTowers);
        GUI.Box(new Rect(Screen.width - 150, 80, 150, 50), "Current Level: " + (GameData.Instance.LevelNumber +1));*/
    }

    public float setDamageToWall(int target, float damage)//Use Id of wall to set Damage to that wall
    {
        WallHealth[target] -= damage;

        if (WallHealth[target] <= 0 && Walls[target] != null) { 

            if (master)
            {
                PhotonNetwork.Instantiate("BOOM!", Walls[target].transform.position, Quaternion.identity, 0);
                PhotonNetwork.Destroy(Walls[target]);
                //Destroy(Walls[target].gameObject);
                Walls[target] = null;
            }
            setTowerDestination();
        }
        return WallHealth[target];
    }
    
    


    private void setTowerDestination()//called after scene Changes so that a new Bake is triggered
    {
        navMesh.RemoveData();//Deletes the current NavMesh
        
        
        for (int i = 0; i< Enemys.Length; i++)
        {
           //Change enemy Target to Tower
            Enemys[i].wallAlive = false;
            Enemys[i].attack = false;
            Enemys[i].setDestination(Tower);
            Enemys[i].idleStart();
        }
        StartCoroutine(BakeNew());
       
    }
    IEnumerator BakeNew()//needed because the  calculation needs some time before a bake can happen
    {
        yield return new WaitForSeconds(0.5f);
        navMesh.BuildNavMesh();//bakes new Nav Mesh
    }
    public static bool getMaster()
    {
        return master;
    }
}
