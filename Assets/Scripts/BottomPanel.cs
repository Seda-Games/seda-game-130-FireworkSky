using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    public Button addButton;
    public Button visitorButton;
    public Button IncomeButton;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    void Init()
    {
        addButton.onClick.AddListener(OnClickAddButton);
        visitorButton.onClick.AddListener(OnClickVisitorButton);
        IncomeButton.onClick.AddListener(OnClickIncomButton);
        PlayerPrefs.GetInt(G.VISITOR, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickAddButton()
    {
        //GameManager.instance.AddFireWorkLevel();
        GameManager.Instance.fireWorkManager.AddFireWork();
    }
    public void OnClickVisitorButton()
    {
        GameManager.Instance.humanManager.AddVisitor();
    }
    public void OnClickIncomButton()
    {
        GameManager.Instance.fireWorkManager.AddIncome();
    }
}
