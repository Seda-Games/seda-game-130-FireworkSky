using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickChangeImage : MonoBehaviour 
{
    Image image;
    [SerializeField] Sprite initialImageSprite, clickImageSprite;

    void Start()
    {
        image = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonClick()
    {
        if(image != null)
        {
            image.sprite = image.sprite == initialImageSprite ? clickImageSprite : initialImageSprite;
        }

    }
    public void ButtonDown()
    {
        if (image != null)
        {
            image.sprite = clickImageSprite;
        }
        //GetComponentInChildren<UIAnimation>()?.ShowAnimation();
        //Invoke("InvokeSetGameObjectChildOrderToLast", 0.1f);
    }
    public void ButtonUp()
    {
        if (image != null)
        {
            image.sprite = initialImageSprite;
        }
        //GetComponentInChildren<UIAnimation>()?.ShowAnimation();
        //Invoke("InvokeSetGameObjectChildOrderToFrist",0.1f);
    }
    void InvokeSetGameObjectChildOrderToFrist()
    {
        Lin.SetGameObjectChildOrder(transform, 1);
    }
    void InvokeSetGameObjectChildOrderToLast()
    {
        Lin.SetGameObjectChildOrder(transform, transform.parent.childCount - 1);
    }
}
