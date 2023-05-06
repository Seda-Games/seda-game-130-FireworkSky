using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxMask : MonoBehaviour
{
    [SerializeField]public List<UIBox> boxs = new List<UIBox>();
    ScrollRect scrollRect;
    [HideInInspector] public UIBox uIBox = null;


    void Start()
    {
        //InitBox();

        scrollRect = GetComponent<ScrollRect>();
        for(int i = 0; i < transform.childCount;++i)
        {
            UIBox uIBox = transform.GetChild(i).GetComponent<UIBox>();
            if (null != uIBox) { boxs.Add(uIBox); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitBox<T>(List<T> list,int index)
    {
        if (index > boxs.Count) { Debug.LogError("输入错误"); };
        boxs[index].InitBox(list);
    }
    public UIBox GetCurUIBox()
    {
        return uIBox;
    }
    public void ShowBox(int index)
    {
        if (boxs.Count == 0) { Debug.Log("没有添加BoxUI"); return; }
        if (uIBox == null ) { uIBox = boxs[0]; }
        index = index >= boxs.Count ? boxs.Count - 1 : index;
        uIBox.HideBox();
        boxs[index].ShowBox();
        uIBox = boxs[index];

        scrollRect = GetComponent<ScrollRect>();
        scrollRect.content = boxs[index].GetComponent<RectTransform>();
    }
}
