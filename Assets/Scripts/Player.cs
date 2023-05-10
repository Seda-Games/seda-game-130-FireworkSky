using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerPhase
{
    UnSelected,Selected, DoNothing
}
public class Player : MonoBehaviour
{
    public Transform Obj;

    public PlayerPhase pp;
    // Start is called before the first frame update
    void Start()
    {
        Obj = this.transform.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void  PlayerMove()
    { }
}
