using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Rocket
{
    public int rocketLevel;
    public GameObject rocketObj;

}

public class FireWorkUI : MonoBehaviour
{
    [SerializeField] List<Rocket> rockets;
    

    public void ShowUI(int level)
    {
        if (!PlayerPrefs.HasKey("Rocket" + level))
        {
            GameManager.instance.playUI.fireworkUI.SetActive(true);
            foreach (var item in rockets)
            {
                item.rocketObj.SetActive(item.rocketLevel == level);
                Debug.Log("亜込込込込込込込込"+ gameObject.name);
            }

            if (SDKManager.Ads.ShowInterstitialAd(true))
            {
                print("ShowInterstitalAd true");
                AdsManager.successCallback += () =>
                {
                    Debug.Log("success");
                };
            }
            else
            {
                Debug.Log("fail");
            }

        }
        
        
    }

    public void ShowButtonUI(int level)
    {
        foreach (var item in rockets)
        {
            item.rocketObj.SetActive(item.rocketLevel == level);
            Debug.Log("dasdasdasda"+gameObject.name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
