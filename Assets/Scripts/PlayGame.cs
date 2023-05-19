using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGame : MonoBehaviour
{
    public List<Transform> transforms;
    public List<GameObject> allCub;
    public GameObject prefab;
    private int currentSpawnIndex = 0;
    public GameObject cub;
    bool isfull = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void AddFireWork()
    {
        if (allCub.Count < transforms.Count) { 
            if (currentSpawnIndex < transforms.Count)
            {
                cub = Instantiate(prefab, transforms[currentSpawnIndex].position, Quaternion.identity);
                cub.transform.parent = GameObject.Find("GameScene/PreparePlaneManager/PreparePlane_" + currentSpawnIndex).transform;
                currentSpawnIndex++;
                allCub.Add(cub);
            }
            else
            {
                currentSpawnIndex = 0;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
   
}
