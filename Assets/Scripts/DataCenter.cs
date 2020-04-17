using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCenter
{
    public GameData gd;
    public Dictionary<string, int> items;

    public DataCenter()
    {
        items = new Dictionary<string, int>();
    }

    public bool HasEnoughItems(string itemKey, int targetAmount = 1)
    {
        return GetItemAmount(itemKey) >= targetAmount;
    }

    public int GetItemAmount(string itemKey)
    {
        if (items.ContainsKey(itemKey))
            return items[itemKey];
        return 0;
    }

    public bool UseItem(string itemKey, int amount = 1)
    {
        if(HasEnoughItems(itemKey, amount))
        {
            items[itemKey] -= amount;
            return true;
        }
        return false;
    }

    public void AddItem(string itemKey, int amount = 1)
    {
        if (items.ContainsKey(itemKey))
            items[itemKey] += amount;
        else
            items[itemKey] = amount;
    }

    public bool HasEnoughMoney(int amount)
    {
        return HasEnoughItems(G.MONEY, amount);
    }

    public int GetMoney()
    {
        return GetItemAmount(G.MONEY);
    }

    public void AddMoney(int amount)
    {
        AddItem(G.MONEY, amount);
    }

    public void UseMoney(int amount)
    {
        UseItem(G.MONEY, amount);
    }

    public void Save()
    {
        string jsonData = JsonUtility.ToJson(new TransformDictionary(items));
        PlayerPrefs.SetString(G.DATA_CENTER, jsonData);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        gd = DataManager.GetInstance().ReadObject<GameData>("game_data");
        gd.Init();

        string jsonData = PlayerPrefs.GetString(G.DATA_CENTER, "");
        if(jsonData.Length > 0)
        {
            items = JsonUtility.FromJson<TransformDictionary>(jsonData).ToDictionary();
        }
    }
}

class TransformDictionary
{
    public List<string> keys;
    public List<int> values;

    public TransformDictionary(Dictionary<string, int> dict)
    {
        keys = new List<string>();
        values = new List<int>();

        foreach (var item in dict)
        {
            keys.Add(item.Key);
            values.Add(item.Value);
        }
    }

    public Dictionary<string, int> ToDictionary()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        for (int i = 0; i < keys.Count; i++)
        {
            dict.Add(keys[i], values[i]);
        }
        return dict;
    }
}
