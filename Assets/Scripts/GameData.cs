using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public LevelData[] levels;
    public AdsConfig adsConfig;

    public Dictionary<int, LevelData> levelDict = new Dictionary<int, LevelData>();
    public UnlockData[] unlockDatas;
    public Dictionary<int, UnlockData> unlockDatasDict = new Dictionary<int, UnlockData>();

    public List<UnlockData> unlockDataList = new List<UnlockData>();
    public int maxRetryAdSeqId;
    public UserControl userControl;
    public ExhibitData[] exhibitDatas;
    public List<ExhibitData> exhibitDatasList = new List<ExhibitData>();
    public Dictionary<int, ExhibitData> exhibitDatasDict = new Dictionary<int, ExhibitData>();


    

    public void Init()
    {
        foreach(var v in levels)
        {
            levelDict[v.id] = v;
        }
        foreach (var v in unlockDatas)
        {
            unlockDatasDict[v.id] = v;
        }
        foreach (var v in exhibitDatas)
        {
            exhibitDatasDict[v.id] = v;
        }
        for (int i = 0; i < exhibitDatas.Length; ++i)
        {
            if (exhibitDatas[i].cost <= 0 || PlayerPrefs.GetInt("Exhibit_Unlock_" + exhibitDatas[i].id) == 1)
            {
                exhibitDatas[i].isUnlock = true;
            }
            if (exhibitDatasList.Count == 0) { exhibitDatasList.Add(exhibitDatas[i]); continue; }
            for (int j = 0; j < exhibitDatasList.Count; ++j)
            {
                if (j == 0 && exhibitDatas[i].sizeValue < exhibitDatasList[j].sizeValue)
                {
                    exhibitDatasList.Insert(0, exhibitDatas[i]);
                    break;
                }
                else if (j == exhibitDatasList.Count - 1)
                {
                    exhibitDatasList.Add(exhibitDatas[i]);
                    break;
                }
                else if (j > 0 && exhibitDatas[i].sizeValue > exhibitDatasList[j - 1].sizeValue && exhibitDatas[i].sizeValue < exhibitDatasList[j].sizeValue)
                {
                    exhibitDatasList.Insert(j, exhibitDatas[i]);
                    break;
                }
            }
        }
        foreach (var v in adsConfig.retryAdSeq)
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
public class UserControl
{
    public int[] money;
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
[System.Serializable]
public class UnlockData
{
    public int id;
    public string typeName;
    public int typeId;
    public int order = 0;
    public int needLevel;
    public int ad = 0;
    public int level = 0;
}
[System.Serializable]

public class ExhibitData
{
    public int id;
    public string name;
    public string modelName;
    public string spriteName;
    public int sizeValue;
    public int cost;
    public int index;
    public bool isUnlock = false;
}
