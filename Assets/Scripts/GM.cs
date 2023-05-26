using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    [SerializeField]
    Button panel, showPanelButton,camerapositionButton1, camerapositionButton2,camerapositionButton3;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("InitGM", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HidePanel()
    {
        panel.gameObject.SetActive(false);
        showPanelButton.gameObject.SetActive(true);
    }
    public void ShowPanel()
    {
        panel.gameObject.SetActive(true);
        showPanelButton.gameObject.SetActive(false);
    }
    public void InitGM()
    {
        panel.onClick.AddListener(HidePanel);
        showPanelButton.onClick.AddListener(ShowPanel);
        camerapositionButton1.onClick.AddListener(CameraToTarget);
        camerapositionButton2.onClick.AddListener(CameraToTarget1);
        camerapositionButton3.onClick.AddListener(CameraToTarget2);
    }
    public void CameraToTarget()
    {
        PlayerPrefs.SetInt(G.STAGE, 1);
        CameraManager.Instance.MoveToTarget();
    }
    public void CameraToTarget1()
    {
        PlayerPrefs.SetInt(G.STAGE, 2);
        CameraManager.Instance.MoveToTarget();
    }
    public void CameraToTarget2()
    {
        PlayerPrefs.SetInt(G.STAGE, 3);
        CameraManager.Instance.MoveToTarget();
    }
}
