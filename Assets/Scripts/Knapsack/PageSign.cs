using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageSign : MonoBehaviour
{
    [SerializeField] Sprite bgSprite, seletctedSprite;
    public List<Button> buttons = new List<Button>();
    int curPage = 0;
    private void Awake()
    {
        InitButtonOnClick();
    }
    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject AddPageSign()
    {
        GameObject pageSign = Instantiate(transform.GetChild(0).gameObject,transform);

        pageSign.GetComponent<Image>().sprite = bgSprite;

        Button button = pageSign.transform.GetComponentInChildren<Button>();
        buttons.Add(button);
        int index = buttons.Count - 1;
        button.onClick.AddListener(() => { ChangeImage(buttons.IndexOf(button)); });

        return pageSign;
    }


    void InitButtonOnClick()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            buttons.Add(transform.GetChild(i).GetComponentInChildren<Button>());
            int index = i;
            buttons[i].onClick.AddListener(() => { ChangeImage(index); });
        }
    }
    public void ChangeImage(int index)
    {
        if (index == curPage || index < 0) { return; }
        index = index >= buttons.Count ? buttons.Count -1: index;
        curPage = index;
        for (int i = 0; i < buttons.Count;++i)
        {
            buttons[i].GetComponent<Image>().sprite = bgSprite;
        }
        buttons[index].GetComponent<Image>().sprite = seletctedSprite;
    }
}
