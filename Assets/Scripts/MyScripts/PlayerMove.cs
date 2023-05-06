using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    Rocker,
    Rrag,
}
public class PlayerMove : MonoBehaviour
{
    public MoveType moveType;
    public float radius = 100;
    public float speedCircleIn;
    public float speedCircleOut;
    GameObject player;
    Vector2 lastMouseClickPos;
    Vector3 playerOffsets = Vector3.zero;

    void Start()
    {
        player = transform.gameObject;
    }

    private void FixedUpdate()
    {
        player.transform.LookAt(transform.position + playerOffsets); 
        player.transform.position += playerOffsets;
        playerOffsets = Vector3.zero;
    }
    void Update()
    {
        
    }
    public void ControlStart(Vector2 pos)
    {
        lastMouseClickPos = pos;


    }
    public void ControlMove(Vector2 pos)
    {
        Vector2 mouseOffsets = pos - lastMouseClickPos;

        float speed = speedCircleIn * Time.fixedDeltaTime;

        if (Vector2.Distance(Vector2.zero, mouseOffsets) > radius) { speed = speedCircleOut * Time.fixedDeltaTime; }

        Vector3 objOffsets = new Vector3(mouseOffsets.x, 0, mouseOffsets.y).normalized * speed;

        playerOffsets = objOffsets;

        if (moveType == MoveType.Rrag) { lastMouseClickPos = pos; }
    }

    public void ControlEnd(Vector2 pos)
    {
    }
    void Limit(GameObject obj)
    {
        Transform rt = obj.transform;
        float up = radius;
        float down = -radius;
        float lift = -radius;
        float right = radius;
        //if (rt.anchoredPosition.x >= up) { rt.anchoredPosition = new Vector2(up, rt.anchoredPosition.y); }
        //else if (rt.anchoredPosition.x <= down) { rt.anchoredPosition = new Vector2(down, rt.anchoredPosition.y); }
        //if (rt.anchoredPosition.y >= right) { rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, right); }
        //else if (rt.anchoredPosition.y <= lift) { rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, lift); }
    }
}
