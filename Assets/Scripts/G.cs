﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class G
{
    public static DataCenter dc = new DataCenter();

    public const string AUDIO = "Audio";
    public const string HAPTICS = "Haptics";
    public const string MONEY = "Money";
    public const string LEVEL = "Level";
    public const string DATA_CENTER = "data_center";
    public const float FLY_COINS_DURATION = 0.5f;

    public const string FIRST_LOGIN_TIME = "first_login_time";
    public const string LOGIN_TIME = "login_time";

    public static string FormatNum(int num)
    {
        if (num > 1000000000)
        {
            return string.Format("{0:0.0}B", num / 1000000000.0f);
        }
        else if (num > 1000000)
        {
            return string.Format("{0:0.0}M", num / 1000000.0f);
        }
        else if (num > 1000)
        {
            return string.Format("{0:0.0}K", num / 1000.0f);
        }
        return num.ToString();
    }

    public static string FormatTime(int num)
    {
        int minutes = num / 60;
        int seconds = num % 60;
        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    public static bool Guess(float probability)
    {
        return Random.Range(0, 1f) > 1f - probability;
    }

    public static void CleanContentContainer(Transform contentContainer)
    {
        foreach (Transform child in contentContainer)
        {
            Object.Destroy(child.gameObject);
        }
    }
}

public static class Scenes
{
    public const string PLAY_SCENE = "PlayScene";
    public const string SHOP_SCENE = "ShopScene";
}

public static class Messages
{
    public const string NO_ADS = "No ads now, try later :(";
    public const string NO_MONEY = "Not enough money :(";
}
