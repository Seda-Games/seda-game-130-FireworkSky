using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Image bg, goodsImage;
    [SerializeField] private Button selectedButton;
    [SerializeField] private Sprite unlockBgSprite, selectedBgSprite;
    [SerializeField] public bool isShow = false,isUnlock = false;
    [HideInInspector] public UnlockData unlockData;
    Sprite lockBgSprite;

    protected void Start()
    {
        lockBgSprite = bg.sprite;
    }
    void Update()
    {

    }
    public virtual void InitData<T>(T type)
    {
        bg = GetComponent<Image>();
        goodsImage = transform.GetChild(0).GetComponent<Image>();
        selectedButton = transform.GetChild(0).GetComponent<Button>();
        goodsImage.GetComponent<Button>().onClick.AddListener(Selected);
        selectedButton.onClick.AddListener(Selected);

        ///********///
        var data = type as UnlockData;
        unlockData = data;
        if (UnlockManager.unlockDataDict[data.id]) { Unlock(); }

    }
    public virtual void ShowGoods()
    {
        isShow = true;
        bg.sprite = unlockBgSprite;
    }
    public virtual void Unlock()
    {
        isUnlock = true;
        PlayerPrefs.SetString(UnlockManager.UNLOCK_+ unlockData.id.ToString(),"true");
    }
    public virtual void Selected()
    {
        if (isUnlock)
        {
            bg.sprite = selectedBgSprite;
        }
    }
    public virtual void CancelSelected()
    {
        if (isUnlock)
        {
            bg.sprite = unlockBgSprite;
        }
    }
    public virtual bool Check(string name)
    {
        return false;
    }
    public virtual void Ad()
    {


    }
}
