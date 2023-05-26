using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : SingleInstance<CameraManager>
{
    Transform cameraRoot, cameraParent, self;
    public Transform prepareRoot;
    public Transform target;
    public Transform target1;
    public Transform target2;
    public Transform preparetar1;
    public Transform preparetar2;
    public Transform preparetar3;
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

        cameraRoot.DOMove(target.position, 1.5f);
        cameraRoot.DORotate(target.eulerAngles,0.1f);
        prepareRoot.DOMove(preparetar1.position,1.5f).OnUpdate(()=> {
            prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
        });
        prepareRoot.eulerAngles = preparetar1.eulerAngles;
    }
    public void Stage2()
    {
        cameraRoot.DOMove(target1.position, 1.5f);
        cameraRoot.DORotate(target1.eulerAngles, 0.1f);
        prepareRoot.DOMove(preparetar2.position, 1.5f).OnUpdate(() => {
            prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
        });
        prepareRoot.eulerAngles = preparetar2.eulerAngles;
    }
    public void Stage3()
    {
        cameraRoot.DOMove(target2.position, 1.5f);
        cameraRoot.DORotate(target2.eulerAngles, 0.1f);
        prepareRoot.DOMove(preparetar3.position, 1.5f).OnUpdate(() => {
            prepareRoot.GetComponent<PreparePlaneManager>().ResetFireWorkPosition();
        });
        prepareRoot.eulerAngles = preparetar3.eulerAngles;
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
    }
}
