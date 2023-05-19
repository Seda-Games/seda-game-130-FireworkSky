using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparePlane : MonoBehaviour
{

    public FireWork fireWork;
    Transform preparePlanePos;
    public bool isexist;
    public int PreparePlaneID;
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
