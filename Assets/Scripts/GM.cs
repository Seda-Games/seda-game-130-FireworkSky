using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    [SerializeField]
    Button panel, showPanelButton,camerapositionButton1, camerapositionButton2,camerapositionButton3, camerapositionButton4, camerapositionButton5;
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
        camerapositionButton4.onClick.AddListener(CameraToTarget3);
        camerapositionButton5.onClick.AddListener(CameraToTarget4);
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
    public void CameraToTarget3()
    {
        PlayerPrefs.SetInt(G.STAGE, 4);
        CameraManager.Instance.MoveToTarget();
    }
    public void CameraToTarget4()
    {
        PlayerPrefs.SetInt(G.STAGE, 5);
        CameraManager.Instance.MoveToTarget();
    }
}
