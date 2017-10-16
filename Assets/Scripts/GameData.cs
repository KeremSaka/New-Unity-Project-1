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
   
}
