using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIBox : MonoBehaviour
{
    [SerializeField] GameObject zeroPage;
    [SerializeField] PageSign pageSign;
    [HideInInspector]public List<Page> pages = new List<Page>();
    [HideInInspector] public int selectedId = 0;
    List<Item> items = new List<Item>();
    RectTransform rectTransform;
    int count = 0;
    float offsetMin;
    void Start()
    {
        if (pages.Count == 0) { AddPage(); }
        rectTransform = GetComponent<RectTransform>();
        pages.First().PlayItemAnimation();

    }

    // Update is called once per frame
    void Update()
    {
        PageSignUpdate();
    }
    public void ShowBox()
    {
        gameObject.SetActive(true);
        pageSign.gameObject.SetActive(true);

    }
    public void HideBox()
    {
        gameObject.SetActive(false);
        pageSign.gameObject.SetActive(false);
    }
    public void InitBox<T>(List<T> list)
    {
        foreach(var v in list)
        {
            AddItem(v);
        }
    }
    public void AddItem<T>(T toolUnlockData)
    {
        if (pages.Count == 0 || count >= pages.Count * pages[0].size)
        {
            AddPage();
        }
        count++;
        Item item = pages.Last().AddItem(toolUnlockData);
        items.Add(item);
    }
    public GameObject AddPage()
    {
        GameObject obj = Instantiate(zeroPage, zeroPage.transform.parent);
        obj.SetActive(true);
        obj.gameObject.name = "Page_" + count;
        Page page = obj.GetComponent<Page>();
        pages.Add(page);
        rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMax = Vector2.right * (zeroPage.GetComponent<RectTransform>().sizeDelta.x * (pages.Count -1));
        rectTransform.offsetMin = Vector2.zero;

        if (pages.Count > 1)
        {
            Button button= pageSign.AddPageSign().GetComponent<Button>();
            int index = pageSign.buttons.Count - 1;
            button.onClick.AddListener(() => TransferPage(index));
        }
        return obj;
    }
    public Item GetItem<T>(T type)
    {

        return null;
    }
    public void ChoiceFirst()
    {
        if (items.Count == 0) { Debug.LogError("选择第一个item失败");return; }
        items[0].Selected();
    }
    public void ChoiceRecord(string name)
    {

        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].Check(name))
            {
                CancelAllItem();
                items[i].Selected();
                break;
            }
        }
    }
    public void CancelAllItem()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            items[i].CancelSelected();
        }
    }
    public Item GetItem(string name)
    {
        for (int i = 0; i < pages.Count; ++i)
        {
            for (int j = 0; j < pages[i].transform.childCount; ++j)
            {
                Item item = pages[i].transform.GetChild(j).GetComponentInChildren<Item>();
                if (item.Check(name))
                {
                    return item;
                }
            }
        }
        return null;
    }
    public void InitPageSignOnClick()
    {
        for(int i = 0; i < pageSign.buttons.Count;++i)
        {
            int index = i;
            pageSign.buttons[i].onClick.AddListener(()=>TransferPage(index));
        }
    }
    public void TransferPage(int index)
    {
        StopCoroutine("TranferPageCoroutine");
        StartCoroutine("TranferPageCoroutine",index);
    }
    public IEnumerator TranferPageCoroutine(int index)
    {
        pageSign.ChangeImage(index);
        float pageSize = zeroPage.transform.GetComponent<RectTransform>().sizeDelta.x + 10;
        float originalSize = pageSize * (pages.Count - 1);
        Vector2 originMin = rectTransform.offsetMin;
        Vector2 originMax = rectTransform.offsetMax;

        float time = 0.2f;
        float timeRatio = 1 / time;
        while(time > 0)
        {
            time -= Time.deltaTime;

            rectTransform.offsetMin = Vector2.Lerp(originMin, -Vector2.right * index * pageSize,1 - time * timeRatio);
            rectTransform.offsetMax = Vector2.Lerp(originMax, rectTransform.offsetMin + Vector2.right * originalSize, 1 - time * timeRatio);

            yield return null;
        }

        yield return null;
    }
    void PageSignUpdate()
    {
        offsetMin = rectTransform.offsetMin.x;
        float pageSize = zeroPage.transform.GetComponent<RectTransform>().sizeDelta.x;
        pageSign.ChangeImage((int)((Mathf.Abs(offsetMin) + pageSize * 0.4f) / pageSize));
    }
    /// <summary>
    /// 随街解锁当前页的某个Item
    /// </summary>
    public void RandomUnlockItem()
    {
        for(int i = 0; i < 1000;++i)
        {
            Item item = items[Random.Range(0, items.Count)];
            if(item.isShow == true && item.isUnlock == false)
            {
                item.Unlock();
                item.Selected();
                break;
            }
            if( i == 999)
            {
                Debug.LogError("没有可以解锁的");
            }
        }

    }
    /// <summary>
    /// 全部解锁完成
    /// </summary>
    /// <returns></returns>
    public bool Unlocked()
    {
        List<Item> lockItems = new List<Item>();
        for (int i = 0; i < items.Count; ++i)
        {
            if (!items[i].isUnlock)
            {
                lockItems.Add(items[i]);
            }
        }
        if (lockItems.Count > 0)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 解锁时的动画
    /// </summary>
    /// <param name="items"></param>
    /// <param name="unlockItem"></param>
    /// <returns></returns>
    IEnumerator UnlockAnimation(List<Item> items, Item unlockItem)
    {
        yield return null;
    }
}
