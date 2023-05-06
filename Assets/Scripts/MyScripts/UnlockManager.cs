using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class UnlockManager
{
    public static bool isInit = false;
    public static Dictionary<int, bool> unlockDataDict = new Dictionary<int, bool>();


    public const string UNLOCK_TYPE_1 = "type1";//解锁类型一
    public const string UNLOCK_TYPE_2 = "type2";//解锁类型二
    public const string UNLOCK_ = "unlock_";//存取解锁物品的前缀名

    /// <summary>
    /// 初始化解锁字典
    /// </summary>
    public static void InitUnlockInformation()
    {
        if (isInit) { return; }
        isInit = true;

        foreach (var v in G.dc.gd.unlockDatasDict.Values)
        {
            if(v.order == 0)
            {
                unlockDataDict[v.id] = true;
            }
            else if (PlayerPrefs.GetString(UNLOCK_+ v.id.ToString(), "false") == "true")
            {
                unlockDataDict[v.id] = true;
            }
            else
            {
                unlockDataDict[v.id] = false;
            }
        }
    }

    /// <summary>
    /// 打印解锁信息
    /// </summary>
    public static void PrintUnlockData()
    {
        Debug.Log("*********************************************************");
        foreach (var v in unlockDataDict)
        {
            Debug.Log(v.Key + "_" + v.Value);
        }
        Debug.Log("*********************************************************");
    }
    /// <summary>
    /// 解锁某个物品
    /// </summary>
    /// <param name="id"></param>
    public static void SetUnlockData(int id)
    {
        unlockDataDict[id] = true;

        PlayerPrefs.SetString(UNLOCK_ + id.ToString(), "true");
    }
    /// <summary>
    /// 当前关的进度是否全部解锁
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static bool IsAllUnlock(int level)
    {
        return G.dc.gd.unlockDataList.Last().level <= level;
    }
    /// <summary>
    /// 获得解锁的信息
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static UnlockData GetUnlockDataFromLevel(int level)
    {
        UnlockData unlockData = null;
        //unlockDataList在gamedata已排序
        for (int i = 0; i < G.dc.gd.unlockDataList.Count;++i)
        {
            if(G.dc.gd.unlockDataList[i].level > level)
            {
                unlockData = G.dc.gd.unlockDataList[i];
                break;
            }
        }
        return unlockData;
    }
    /// <summary>
    /// 根据解锁表返回关联表的信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typeId"></param>
    /// <returns></returns>
    public static T GetUnlockData<T>(int id)
    {
        T t = default(T);
        UnlockData unlockData = G.dc.gd.unlockDatasDict[id];
        switch (unlockData.typeName)
        {
            case UNLOCK_TYPE_1:
            //    t = (T)(object)G.dc.gd.armsDatasDict[unlockData.typeId];
                break;
            case UNLOCK_TYPE_2:
            //    t = (T)(object)G.dc.gd.wormManualDatasDict[unlockData.typeId];
                break;
        }
        return t;
    }
    /// <summary>
    /// 获得id为 ？ 解锁的Icon名字
    /// </summary>
    /// <returns></returns>
    public static string GetUnlockDataIconName(int id)
    {
        string name = null;

        UnlockData unlockData = G.dc.gd.unlockDatasDict[id];

        switch (unlockData.typeName)
        {
            case UNLOCK_TYPE_1:
                break;
            case UNLOCK_TYPE_2:
                break;

        }
        return name;
    }
    /// <summary>
    /// 获得解锁物品的模型名字
    /// </summary>
    /// <returns></returns>
    public static string GetReadyUnlockModelName(int id)
    {
        string name = "";

        UnlockData unlockData = G.dc.gd.unlockDatasDict[id];

        switch (unlockData.typeName)
        {
            case UNLOCK_TYPE_1:
                break;
            case UNLOCK_TYPE_2:
                break;
        }
        return name;
    }
    /// <summary>
    /// 获得准备解锁的Icon名字
    /// </summary>
    /// <returns></returns>
    public static string GetReadyUnlockDataIconName()
    {
        string name = "";

        UnlockData unlockData =  GetUnlockDataFromLevel(GameManager.Instance.curLevel);

        switch(unlockData.typeName)
        {
            case UNLOCK_TYPE_1:
                break;
            case UNLOCK_TYPE_2:
                break;
        }
        return name;
    }

    /// <summary>
    /// 获得准备解锁的物品名字
    /// </summary>
    /// <returns></returns>
    public static string GetReadyUnlockDataName(int id)
    {
        string name = "";

        UnlockData unlockData = G.dc.gd.unlockDatasDict[id];

        switch (unlockData.typeName)
        {
            case UNLOCK_TYPE_1:
                break;
            case UNLOCK_TYPE_2:
                break;
        }
        return name;
    }



    /// <summary>
    /// 获得解锁进度
    /// </summary>
    /// <param name="level"></param>
    /// <returns>从progress[0]涨至progress[1]</returns>
    public static float[] GetCurLevelUnlockProgress(int level)
    {
        float[] progress = new float[2];
        progress[0] = 0;
        progress[1] = 1.01f;
        List<UnlockData> unlockDataList = G.dc.gd.unlockDataList;
        for (int i = 0; i < unlockDataList.Count; ++i)
        {
            if (unlockDataList[i].level > level)
            {
                float needLevel = unlockDataList[i].needLevel;
                if(unlockDataList[i -1 ].level == 0)
                {
                    progress[0] = (level -1) / needLevel;
                    progress[1] = (level ) / needLevel;
                }
                else
                {
                    progress[0] = (level - unlockDataList[i - 1].level) / needLevel;
                    progress[1] = (level + 1 - unlockDataList[i - 1].level )/ needLevel;
                }
                break;
            }
            if( i == unlockDataList.Count -1)
            {
                Debug.Log("没获取倒解锁进度");

                return null;
            }
        }
        return progress;
    }
}
