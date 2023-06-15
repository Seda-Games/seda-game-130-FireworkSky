using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : SingleInstance<CameraManager>
{
    Transform cameraRoot, cameraParent, self;
    public Transform prepareRoot;
    public Transform[] target;
    public float speed = 1;
    Vector3 distancePlayer;
    public Vector3 smooth;
    
    private void Awake()
    {
        cameraRoot = transform.root;
        cameraParent = transform.parent;
        self = transform;
    }
    void Start()
    {
        //target = GameManager.Instance.player.transform;
        //distancePlayer = self.position - target.position;
        MoveToTarget();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void LateUpdate()
    {
       

    }
    public void Stage1()
    {

        /*prepareRoot.DOMove(target[0].position, 1.5f).OnUpdate(() => {
            prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
        });*/
        //prepareRoot.eulerAngles = target[0].eulerAngles;
        //prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
        cameraRoot.DOMove(target[0].position, 1.5f);
        cameraRoot.DORotate(target[0].eulerAngles,0.1f);
        
    }
    public void Stage2()
    {
        /*prepareRoot.DOMove(target[0].position, 1.5f).OnUpdate(() => {
             prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
         });*/
        //prepareRoot.eulerAngles = target[1].eulerAngles;

        cameraRoot.DOMove(target[1].position, 1.5f);
        cameraRoot.DORotate(target[1].eulerAngles, 0.1f);
       
    }
    public void Stage3()
    {
        /*prepareRoot.DOMove(target[0].position, 1.5f).OnUpdate(() => {
            prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
        });*/
        //prepareRoot.eulerAngles = target[2].eulerAngles;
        //prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
        cameraRoot.DOMove(target[2].position, 1.5f);
        cameraRoot.DORotate(target[2].eulerAngles, 0.1f);
        
    }
    public void Stage4()
    {
        /*prepareRoot.DOMove(target[0].position, 1.5f).OnUpdate(() => {
             prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
         });*/
        //prepareRoot.eulerAngles = target[3].eulerAngles;
        //prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
        cameraRoot.DOMove(target[3].position, 1.5f);
        cameraRoot.DORotate(target[3].eulerAngles, 0.1f);

    }
    public void Stage5()
    {
        /*prepareRoot.DOMove(target[0].position, 1.5f).OnUpdate(() => {
             prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
         });*/
        //prepareRoot.eulerAngles = target[3].eulerAngles;
        //prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
        cameraRoot.DOMove(target[4].position, 1.5f);
        cameraRoot.DORotate(target[4].eulerAngles, 0.1f);

    }
    public void MoveToTarget()
    {
        if (PlayerPrefs.GetInt(G.STAGE, 1) == 1)
        {
            Stage1();
        }
        else if (PlayerPrefs.GetInt(G.STAGE, 1) == 2)
        {
            Stage2();
        }
        else if (PlayerPrefs.GetInt(G.STAGE, 1) == 3)
        {
            Stage3();
        }
        else if (PlayerPrefs.GetInt(G.STAGE, 1) == 4)
        {
            Stage4();
        }
        else if (PlayerPrefs.GetInt(G.STAGE, 1) == 5)
        {
            Stage5();
        }
    }
}
