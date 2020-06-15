using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public LevelData[] levels;
    public AdsConfig adsConfig;

    public Dictionary<int, LevelData> levelDict = new Dictionary<int, LevelData>();

    public int maxRetryAdSeqId;

    public void Init()
    {
        foreach(var v in levels)
        {
            levelDict[v.id] = v;
        }

        foreach(var v in adsConfig.retryAdSeq)
        {
            if (v > maxRetryAdSeqId)
                maxRetryAdSeqId = v;
        }
    }

    public LevelData GetLevelData(int curLevel)
    {
        if (curLevel <= levels.Length)
            return levels[curLevel-1];
        return FakeLevelData(curLevel);
    }

    public LevelData FakeLevelData(int curLevel)
    {
        var ld = new LevelData();
        return ld;
    }
}

[System.Serializable]
public class LevelData
{
    public int id;
    public int showAd = 0;
}

[System.Serializable]
public class AdsConfig
{
    public int masterSwitch;
    public int[] retryAdSeq;
    public int retryAdPeriod;
    public int noNextLevelAdsDays;
    public int nextLevelSafeDuration;
}

