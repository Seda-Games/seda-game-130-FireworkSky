using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockPlane : MonoBehaviour
{
    public GameObject plane, lockUI,unlockUI;
    public Image unlockBgImage, unlockImage;
    void Start()
    {
        
    }

    void InitUnlockData(LevelData levelData)
    {
        string spriteNmaes = UnlockManager.GetUnlockDataIconName(levelData.id);
        Sprite[] sprites = new Sprite[2];
        string path = "Sprite";
        string silhouette = "";//剪影后缀
        sprites[0] = Resources.Load<Sprite>(path + spriteNmaes);
        sprites[1] = Resources.Load<Sprite>(path + spriteNmaes+ silhouette);
        unlockBgImage.sprite = sprites[0];
        unlockImage.sprite = sprites[1];
    }
    public void ShowUnlockPlane()
    {
        plane.SetActive(true);
    }
    public void HideUnlockPlane()
    {
        plane.SetActive(false);
    }
    void PlayUnlockAnimation(int level)
    {
        float[] progress = UnlockManager.GetCurLevelUnlockProgress(level) ;
        StartCoroutine(PlayUnlockAnimationCoroutine(progress[0], progress[1]));
    }

    IEnumerator PlayUnlockAnimationCoroutine(float original,float aim)
    {
        float time = 1;
        float timeRatio = 1 / time;
        while (time > 0)
        {
            time -= Time.deltaTime;
            unlockImage.fillAmount = Mathf.Lerp(original, aim,1 - time * timeRatio);
            yield return null;
        }
        yield return null;
        if(aim >= 1)
        {
            //解锁界面
            lockUI.SetActive(false);
            unlockUI.SetActive(true);
        }
    }
}
