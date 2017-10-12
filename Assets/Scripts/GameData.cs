using UnityEngine;
using System.Collections;

public class GameData
{
    private static GameData instance;

    public GameData()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }


    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                new GameData();
            }
            return instance;
        }
    }


    //private bool paused = false;

    public int NumberOfTowers
    {
        get;
        set;
    }


    public int EnemyKilled
    {
        get;
        set;
    }

    public int EnemyReachedDestination
    {
        get;
        set;
    }

    public int MaxEnemyNumber
    {
        get;
        set;
    }

    public int LevelNumber
    {
        get;
        set;
    }

    public int Enemyspointer
    {
        get;
        set;
    }



    public GameObject[] Enemys
    {
        get;
        set;
    }
    public GameObject Tower
    {
        get;
        set;
    }
}
