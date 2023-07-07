using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SDKManager
{
    public static AdsManager Ads;
    public static void InitAdUnitySDK()
    {
        GameObject go = GameObject.Find("AdsManager");
        if (go != null)
        {
            Ads = go.GetComponent<AdsManager>();
            Ads.InitializeAds();

        }
        else
        {
            Debug.Log("AdsManager is NULL");
        }
    }
}
