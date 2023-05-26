using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingleInstance<CameraManager>
{
    Transform cameraRoot, cameraParent, self;
    public Transform target;
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
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        //cameraRoot.position = Vector3.SmoothDamp(cameraRoot.position, target.position, ref smooth, speed ); ;
    }
}
