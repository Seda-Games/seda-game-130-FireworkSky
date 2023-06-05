using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparePlane : MonoBehaviour
{

    public FireWork fireWork;
    Transform preparePlanePos;
    public bool isexist;
    public int PreparePlaneID;
    public bool isUnlock;
    public GameObject prepareLock;
    public Text unlockcost;
    // Start is called before the first frame update
    void Start()
    {
        preparePlanePos = this.transform.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
