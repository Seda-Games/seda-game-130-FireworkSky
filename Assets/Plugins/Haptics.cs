using UnityEngine;
using System.Runtime.InteropServices;

public enum HapticIntensity
{
    Light, Medium, Heavy
};

public static class Haptics
{
#if UNITY_IOS
    [DllImport("__Internal")]
    public static extern void SetVibratorIOS(int intensityLevel);
#elif UNITY_ANDROID
    private static bool androidInited = false;
    private static int HapticFeedbackLight, HapticFeedbackMedium, HapticFeedbackHeavy;
    private static AndroidJavaObject UnityPlayer;
#endif

    /// <summary>
    /// Feedback dfifferently based on intensity form 0 to 1.
    /// </summary>
    /// <param name="intensity"></param>
    public static void Feedback(float intensity = 0.5f)
    {
        int i = (int)(Mathf.Abs(intensity) / 0.33f);
        HapticIntensity hi = HapticIntensity.Light + Mathf.Min(2, i);
        Feedback(hi);
    }

    public static void Feedback(HapticIntensity hapticIntensity)
    {
        int hapticsOn = PlayerPrefs.GetInt("Haptics", 1);
        if (hapticsOn == 0)
            return;
        
#if UNITY_EDITOR
        Debug.Log("Haptics::Feedback! Intensity Level:" + hapticIntensity);
#elif UNITY_IOS
        SetVibratorIOS(hapticIntensity - HapticIntensity.Light);
#elif UNITY_ANDROID
        SetVibratorAndroid(hapticIntensity - HapticIntensity.Light);
#endif
    }

#if UNITY_ANDROID
    static void InitVibratorAndroid()
    {
        androidInited = true;
        var ajc = new AndroidJavaClass("android.view.HapticFeedbackConstants");
        HapticFeedbackLight = ajc.GetStatic<int>("LONG_PRESS");
        HapticFeedbackMedium = ajc.GetStatic<int>("VIRTUAL_KEY");
        HapticFeedbackHeavy = ajc.GetStatic<int>("KEYBOARD_TAP");
        UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
        Debug.Log("Haptics::InitVibratorAndroid!");
        //Alternative way to get the UnityPlayer:
        //int content=new AndroidJavaClass("android.R$id").GetStatic<int>("content");
        //new AndroidJavaClass ("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("findViewById",content).Call<AndroidJavaObject>("getChildAt",0);
    }

    static void SetVibratorAndroid(int intensityLevel)
    {
        if (!androidInited)
            InitVibratorAndroid();
        UnityPlayer.Call<bool>("performHapticFeedback",
            intensityLevel == 0 ? HapticFeedbackLight : (intensityLevel == 1 ? HapticFeedbackMedium : HapticFeedbackHeavy));
        //Debug.Log("Haptics::Android Feedback!");
    }
#endif
}
