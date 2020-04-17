using UnityEngine;
using System.Runtime.InteropServices;

public static class Haptics
{
#if UNITY_IOS
    [DllImport("__Internal")]
    public static extern void setVibratorIOS();
#elif UNITY_ANDROID
    private static bool androidInited = false;
    private static int HapticFeedbackConstantsKey;
    private static AndroidJavaObject UnityPlayer;
#endif

    public static void Feedback()
    {
        int hapticsOn = PlayerPrefs.GetInt("Haptic", 1);
        if (hapticsOn == 0)
            return;
#if UNITY_EDITOR
        //Debug.Log("Haptics::Feedback!");
#elif UNITY_IOS
        setVibratorIOS();
#elif UNITY_ANDROID
        setVibratorAndroid();
#endif
    }

#if UNITY_ANDROID
    static void InitVibratorAndroid()
    {
        HapticFeedbackConstantsKey = new AndroidJavaClass("android.view.HapticFeedbackConstants").GetStatic<int>("VIRTUAL_KEY");
        UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
        Debug.Log("Haptics::InitVibratorAndroid!");
        //Alternative way to get the UnityPlayer:
        //int content=new AndroidJavaClass("android.R$id").GetStatic<int>("content");
        //new AndroidJavaClass ("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("findViewById",content).Call<AndroidJavaObject>("getChildAt",0);
    }

    static void setVibratorAndroid()
    {
        if (!androidInited)
            InitVibratorAndroid();
        UnityPlayer.Call<bool>("performHapticFeedback", HapticFeedbackConstantsKey);
        Debug.Log("Haptics::Android Feedback!");
    }
#endif
}
