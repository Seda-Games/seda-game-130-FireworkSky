using SupersonicWisdomSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class SDKManager
{
    public static void supersonicInit(UnityAction callback)
    {
        SupersonicWisdom.Api.AddOnReadyListener(callback.Invoke);
        SupersonicWisdom.Api.Initialize();
    }


    public static void CompleteLevel(int curLevel)
    {
        SupersonicWisdom.Api.NotifyLevelCompleted(curLevel, null);


    }

    public static void FailLevel(int curLevel)
    {
        SupersonicWisdom.Api.NotifyLevelFailed(curLevel, null);


    }
    public static void StartLevel(int curLevel)
    {
        SupersonicWisdom.Api.NotifyLevelStarted(curLevel, null);


    }
}
