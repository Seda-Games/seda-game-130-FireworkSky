using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockerPanel : MonoBehaviour
{
    public bool isActive = true;
    [SerializeField] GameObject panel;
    [SerializeField] RectTransform rocker,rockerBg;

    float radius;
    void Start()
    {
        radius = rockerBg.rect.width / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetRcokerPanel(bool isTrue)
    {
        gameObject.SetActive(true);
        panel.gameObject.SetActive(isTrue);
    }
    public void ShowRocker()
    {
        SetRcokerPanel(true);
    }
    public void HideRocker()
    {
        SetRcokerPanel(false);
    }
    public void SetRockerBgPos(Vector2 pos)
    {
        if (isActive) { ShowRocker(); }

        rockerBg.position = pos;
    }
    public void SetRockerPos(Vector2 pos)
    {
        rocker.position = pos;
        Limit(rocker.gameObject);
    }
    void Limit(GameObject obj)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        float up = radius;
        float down = -radius;
        float lift = -radius;
        float right = radius;
        //if (rt.anchoredPosition.x >= up) { rt.anchoredPosition = new Vector2(up, rt.anchoredPosition.y); }
        //else if (rt.anchoredPosition.x <= down) { rt.anchoredPosition = new Vector2(down, rt.anchoredPosition.y); }
        //if (rt.anchoredPosition.y >= right) { rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, right); }
        //else if (rt.anchoredPosition.y <= lift) { rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, lift); }
        if(Vector2.Distance(Vector2.zero,rt.anchoredPosition) >= radius){ rt.anchoredPosition = rt.anchoredPosition.normalized * 100; }
    }
}
