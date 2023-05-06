using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectPool : Object
{

    public GameObject gameObject;
    public List<GameObject> livePool = new List<GameObject>();
    public List<GameObject> deathPool = new List<GameObject>();

    private GameObjectPool() { }

    public GameObjectPool(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
    public GameObject GetGameObject()
    {
        if(deathPool.Count <= 0)
        {
            return AddGameObject();
        }

        GameObject obj = deathPool.First();

        deathPool.Remove(obj);

        livePool.Add(obj);

        obj.SetActive(true);

        return obj;
    }
    private GameObject AddGameObject()
    {
        GameObject obj = Instantiate(gameObject);

        obj.SetActive(true);

        livePool.Add(obj);

        return obj;
    }
    public void RemoveGameObject(GameObject gameObject)
    {
        deathPool.Add(gameObject);

        livePool.Remove(gameObject);

        gameObject.SetActive(false);
    }
    public void Clean()
    {
        for(int i = livePool.Count-1; i >=0;++i)
        {
            GameObject obj = livePool[i];
            livePool.Remove(obj);
            Destroy(obj);
        }
        for (int i = deathPool.Count - 1; i >= 0; ++i)
        {
            GameObject obj = deathPool[i];
            deathPool.Remove(obj);
            Destroy(obj);
        }
    }
}
