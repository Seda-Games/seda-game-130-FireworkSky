using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SkyboxModel
{
    public int skyboxLevel;
    public GameObject skyboxObj;

}
public class Map : MonoBehaviour
{
    [SerializeField] List<SkyboxModel> skyboxModels;


    public void ShowModel(int map)
    {
        foreach (var item in skyboxModels)
        {
            item.skyboxObj.SetActive(item.skyboxLevel == map);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
