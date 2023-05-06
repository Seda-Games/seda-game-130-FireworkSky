using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Lin:
/// 使用unity的api完成刘海屏适配
/// 使用需要将最外层的UI的轴点设置为最上方并且锚点设置为全屏
/// 设置好后即可使用
/// </summary>
public class SafeArea_All : MonoBehaviour
{
    Rect safeArea;
    public float safeAreaHeight;
    private void Awake()
    {
        safeArea = Screen.safeArea;
        RectTransform rt = transform.GetComponent<RectTransform>();
        float safeAreaHeight = Screen.height - safeArea.height;
        //rt.anchoredPosition -= new Vector2(0, safeAreaHeight);
        //rt.sizeDelta -= new Vector2(0, safeAreaHeight);
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, -safeAreaHeight);
        this.safeAreaHeight = safeAreaHeight;
        GameObject safeImage = transform.root.transform.Find("Safe_Image").gameObject;
        safeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, safeAreaHeight);
    }
    void Start()
    {

    }
    

}
