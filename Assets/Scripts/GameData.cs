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
    public FireWorkData[] fireWorkDatas;
    public Dictionary<int, FireWorkData> fireWorkDataDict = new Dictionary<int, FireWorkData>();
    public HumanData[] humanDatas;
    public Dictionary<int, HumanData> humanDataDataDict = new Dictionary<int, HumanData>();
    public AddIncomeData[] addIncomeDatas;
    public Dictionary<int, AddIncomeData> AddIncomeDataDict = new Dictionary<int, AddIncomeData>();
    public AddFireWorkData[] addFireWorkDatas;
    public Dictionary<int, AddFireWorkData> addFireWorkDataDict = new Dictionary<int, AddFireWorkData>();
    public UnlockFirePlaneData[] unlockFirePlaneDatas;
    public Dictionary<int, UnlockFirePlaneData> unlockFirePlaneDataDict = new Dictionary<int, UnlockFirePlaneData>();
    public FirworkPlaneTable[] firworkPlaneTables;
    public Dictionary<int, FirworkPlaneTable> firworkPlaneTableDict = new Dictionary<int, FirworkPlaneTable>();
    public PreparePlaneTable[] preparePlaneTables;
    public Dictionary<int, PreparePlaneTable> preparePlaneTableDict = new Dictionary<int, PreparePlaneTable>();
    public AchievementTable[] achievementTables;
    public Dictionary<int, AchievementTable> achievementTableDict = new Dictionary<int, AchievementTable>();
    public UnlockFirePlaneTable[] unlockFirePlaneTables;
    public Dictionary<int, UnlockFirePlaneTable> unlockFirePlaneTableDict = new Dictionary<int, UnlockFirePlaneTable>();
    public void Init()
    {
        foreach(var v in levels)
        {
            levelDict[v.id] = v;
        }
        foreach (var v in fireWorkDatas)
        {
            fireWorkDataDict[v.level] = v;
        }
        foreach (var v in humanDatas)
        {
            humanDataDataDict[v.level] = v;
        }
        foreach (var v in addIncomeDatas)
        {
            AddIncomeDataDict[v.level] = v;
        }
        foreach (var v in addFireWorkDatas)
        {
            addFireWorkDataDict[v.level] = v;
        }
        foreach (var v in firworkPlaneTables)
        {
            firworkPlaneTableDict[v.level] = v;
        }
        foreach (var v in preparePlaneTables)
        {
            preparePlaneTableDict[v.prepareid] = v;
        }
        foreach (var v in achievementTables)
        {
            achievementTableDict[v.level] = v;
        }
        /*foreach (var v in unlockDatas)
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
        }*/
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
    public int mapId;
    public int firelevel;
    public int money;
    public int limitmaxlevel;
    public int nextmapcost;
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
[System.Serializable]
public class FireWorkData
{
    public int level;
    public int income;
    public int humanincome;
}
[System.Serializable]

public class HumanData
{
    public int level;
    public float flow;
    public float second;
    public int cost;
    public int income;
}
[System.Serializable]
public class AddIncomeData
{
    public int level;
    public int income;
    public int cost;
}
[System.Serializable]
public class AddFireWorkData
{
    public int level;
    public int cost;
}

[System.Serializable]
public class UnlockFirePlaneData
{
    public int id;
    public int unlocklevel;
    public int unlockcost;
}
[System.Serializable]
public class FirworkPlaneTable
{
    public int level;
    public int unlockcost;
}
[System.Serializable]
public class PreparePlaneTable
{
    public int prepareid;
    public int unlockcost;
    public int isunlock;
}

[System.Serializable]
public class AchievementTable
{
    public int level;
    public int accumulatelauncher;
    public int accumulatehuman;
    public float duration;
    public int multiple;
}

[System.Serializable]
public class UnlockFirePlaneTable
{
    public int fireplaneid;
    public int unlockcost;
    public int isunlock;
}