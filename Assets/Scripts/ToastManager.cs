using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToastManager
{
    static GameObject canvas;
    static Transform theToast;
    static Toast curToast;

    static void Prepare()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas");
            theToast = canvas.transform.Find("Toast");
            curToast = theToast.GetComponent<Toast>();
        }
    }

    public static void Show(string text, float duration = 1f)
    {
        Prepare();
        theToast.SetAsLastSibling();
        curToast.ShowToast(text, duration);
    }
}
