using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : SingleInstance<GameSceneManager>
{
    public GameObject plane,testSceneModel;
    private void Awake()
    {
        Destroy(testSceneModel);
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void InitSceneModel(string pathName)
    {
        GameObject planeModel = Instantiate(Resources.Load<GameObject>(pathName), plane.transform);
    }
}
