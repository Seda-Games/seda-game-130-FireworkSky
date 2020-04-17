using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UserInput
{
    bool moving;
    Vector3 prevPos;
    float threshold = 1f;

    UnityAction<Vector2> controlStart, controlMove, controlStationary, controlEnd;

    public UserInput(
        UnityAction<Vector2> controlStart,
        UnityAction<Vector2> controlMove,
        UnityAction<Vector2> controlEnd
        ) : this(controlStart, controlMove, null, controlEnd)
    {
    }

    public UserInput(
        UnityAction<Vector2> controlStart,
        UnityAction<Vector2> controlMove,
        UnityAction<Vector2> controlStationary,
        UnityAction<Vector2> controlEnd
        )
    {
        this.controlStart = controlStart;
        this.controlMove = controlMove;
        this.controlStationary = controlStationary;
        this.controlEnd = controlEnd;
    }

    public void UserControl()
    {
        // Mobile Control
        if (Input.touchCount > 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        controlStart?.Invoke(touch.position);
                        break;
                    case TouchPhase.Moved:
                        controlMove?.Invoke(touch.position);
                        break;
                    case TouchPhase.Stationary:
                        controlStationary?.Invoke(touch.position);
                        break;
                    case TouchPhase.Ended:
                        controlEnd?.Invoke(touch.position);
                        break;
                }
            }
        }
        else // Desktop Control
        {
            Vector3 curPos = Input.mousePosition;
            if (Input.GetMouseButton(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (!moving)
                    {
                        moving = true;
                        controlStart?.Invoke(curPos);
                    }
                    if(Vector3.Distance(curPos, prevPos) < threshold)
                        controlStationary?.Invoke(curPos);
                    else
                        controlMove?.Invoke(curPos);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    moving = false;
                    controlEnd?.Invoke(curPos);
                }
            }
            prevPos = curPos;
        }
    }
}