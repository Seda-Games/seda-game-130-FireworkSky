using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private static DataManager instance;

    public static DataManager GetInstance()
    {
        if (instance == null)
        {
            instance = new DataManager();
        }
        return instance;
    }

    // Prevent external construction
    private DataManager() { }

    public T ReadObject<T>(string filename)
    {
        string data = Resources.Load<TextAsset>(filename).text;
        return JsonUtility.FromJson<T>(data);
    }
}