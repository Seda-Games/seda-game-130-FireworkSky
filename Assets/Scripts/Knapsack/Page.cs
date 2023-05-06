using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    public int size = 6;
    [HideInInspector] public int count = 0;
    List<Item> items = new List<Item>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Item AddItem<T>(T toolUnlockData)
    {
        Item item = transform.GetChild(count).GetComponent<Item>();
        items.Add(item);
        item.InitData(toolUnlockData);
        count++;
        return item;
    }
    public void PlayItemAnimation()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<UIAnimation>().ShowAnimation( i * Time.fixedDeltaTime * 3);
        }
    }

}
