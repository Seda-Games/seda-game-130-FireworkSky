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

    Animator animator;

    ParticleSystem particle;
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

    public void PlayFx()
    { 
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        particle = this.transform.GetChild(1).GetComponent<ParticleSystem>();
        animator.enabled=true;
        particle.Play();
    }
}
