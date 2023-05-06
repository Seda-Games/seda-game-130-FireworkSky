using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckView : MonoBehaviour
{
    public GameObject checkObj;
    public float angle = 70;
    public float distance = 5;
    bool isCheck = false;
    void Start()
    {
        isCheck = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isCheck)
        {
            if(Vector3.Distance(transform.position, checkObj.transform.position) <= distance)
            {
                Vector3 objDirection = checkObj.transform.position - transform.position;
                Vector3 selfDirection = transform.forward;

                //Debug.Log(Vector2.Angle(new Vector2(selfDirection.x, selfDirection.z), new Vector2(objDirection.x, objDirection.z)));
                if (Vector2.Angle(new Vector2(selfDirection.x, selfDirection.z), new Vector2(objDirection.x, objDirection.z)) < angle/2.0f)
                {
                    Vector3 selfPos = new Vector3(transform.position.x,0.1f, transform.position.z);
                    RaycastHit hit;
                    if(Physics.Raycast(selfPos, objDirection,out hit ,Vector3.SqrMagnitude(objDirection * 2)))
                    {
                        if(hit.transform.tag.Equals(G.PLAYER))
                        {
                            Debug.Log("追击");
                            isCheck = false;
                        }

                    }

                }
            }
        }
    }
    //两点的角度
    public float PointToAngle(Vector2 p1, Vector2 p2)
    {
        Vector2 p;
        p.x = p2.x - p1.x;
        p.y = p2.y - p1.y;
        return Mathf.Atan2(p.y, p.x) * 180 / Mathf.PI;
    }
}
