using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsBase : MonoBehaviour
{
    public CallBack finishCallback = null;

    public virtual void OnClick() { }
}
