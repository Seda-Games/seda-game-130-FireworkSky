using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGame : MonoBehaviour
{
    public List<Transform> transforms;

    public GameObject prefab;
    private int currentSpawnIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void AddFireWork()
    {
        if (currentSpawnIndex < transforms.Count)
        {
            Instantiate(prefab,transforms[currentSpawnIndex].position,Quaternion.identity);
            currentSpawnIndex++;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
