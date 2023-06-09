using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BtnAni : MonoBehaviour
{
    Button BTN;
    const float time = 0.08f;
    bool isOnBtn = false;
    public Text text;
    Vector2 originpos;
    private void Start()
    {
        BTN = GetComponent<Button>();
        BTN.onClick.AddListener(AnitoButton);
        originpos = text.rectTransform.anchoredPosition;


    }
    public void AnitoButton()
    {
        // this.transform.DOKill();
        //if (!BTN.interactable) return;
        if (isOnBtn) return;
        isOnBtn = true;
        this.transform.DOScale(0.9f, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            this.transform.DOScale(1f, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                isOnBtn = false;
            });
        });
    }
    private void OnDestroy()
    {
        BTN?.onClick.RemoveAllListeners();
    }
    //·Å´ó
    public void Amplify()
    {
        this.transform.DOScale(0.9f, time).SetEase(Ease.Linear);
    }
    //»Øµ¯
    public void Reduce()
    {
        this.transform.DOScale(1f, time).SetEase(Ease.Linear);
    }
    public void OnMouseDown()
    {
        text.rectTransform.anchoredPosition = new Vector2(0, 9f);
    }
    public void OnMouseUp()
    {
        text.rectTransform.anchoredPosition = originpos;
    }
}
