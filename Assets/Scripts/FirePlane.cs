using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirePlane : MonoBehaviour
{

    public FireWork fireWork;
    public bool isexist;
    public int FirePlaneID;
    public GameObject Lock, Unlock;
    public bool isUnlock;
    public Text unlockcost;
    // Start is called before the first frame update


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowOrHideLock(bool isunlock)
    {
        Lock.SetActive(isunlock);
        Unlock.SetActive(!isunlock);
    }
}
