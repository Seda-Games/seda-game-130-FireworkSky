using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitch : MonoBehaviour
{
    [SerializeField]public List<Button> buttons = new List<Button>();
    void Start()
    {
        UpdataButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdataButtons()
    {
        foreach (var v in buttons)
        {
            v?.onClick.AddListener(() => { ClickButton(v); });
        }
    }
    void ClickButton(Button button)
    {
        ButtonClickChangeImage[] buttonClickChangeImages = button.GetComponentsInChildren<ButtonClickChangeImage>();

        for (int i = 0; i < buttons.Count && buttons[i] != null; ++i)
        {
            ButtonClickChangeImage[] ClickChangeImages = buttons[i]?.GetComponentsInChildren<ButtonClickChangeImage>();
            for(int j = 0; j < ClickChangeImages.Length && null != ClickChangeImages[j]; ++j)
            {
                for(int k = 0;k < buttonClickChangeImages.Length;++k)
                {
                    if (buttonClickChangeImages[k] == ClickChangeImages[j])
                    {
                        continue;
                    }
                    if(k == buttonClickChangeImages.Length - 1)
                    {
                        ClickChangeImages[j]?.ButtonUp();
                    }
                }
            }
        }
        foreach (var vb in buttonClickChangeImages)
        {
            vb.ButtonDown();
        }
    }
}
