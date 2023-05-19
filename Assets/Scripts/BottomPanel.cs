using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    public Button addButton;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    void Init()
    {
        addButton.onClick.AddListener(OnClickAddButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickAddButton()
    {
        GameManager.Instance.fireWorkManager.AddFireWork();
    }

}
