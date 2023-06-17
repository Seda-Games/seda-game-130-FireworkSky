using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class OriginalAni : MonoBehaviour
{
    Button BTN;
    const float time = 0.08f;
    bool isOnBtn = false;
    Vector2 originpos;
    private void Start()
    {
        BTN = GetComponent<Button>();
        BTN.onClick.AddListener(AnitoButton);
        


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
   
}
