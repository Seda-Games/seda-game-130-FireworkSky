using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public Sprite spOn, spOff;

    [HideInInspector]
    public bool IsOn = true;

    Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle()
    {
        AudioManager.instance?.Tap();
        IsOn = !IsOn;
        if (IsOn)
        {
            img.sprite = spOn;

        }
        else
        {
            img.sprite = spOff;
        }
    }
}