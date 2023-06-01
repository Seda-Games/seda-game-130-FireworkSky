using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewbieGuide : MonoBehaviour
{
    private enum State
    {
        Step1, Step2, Step3, Step4, Step5
    }
    private State currentState = State.Step1;
    public bool m_bIsNewbieGuide;
    public Button buyFireworkbutton;
    public GameObject[] newbie;
    private int clickCount = 0;
    private int clickCount1 = 0;
    private bool isGuiding = true;
    private bool islastStep = false;
    private bool isfinishStep = false;
    private bool isfinishStep1 = false;
    private bool isfinish = false;
    // Start is called before the first frame update
    void Start()
    {
        m_bIsNewbieGuide= PlayerPrefs.GetInt(G.NEWBIEGUIDE, 0) != 1;
        if (m_bIsNewbieGuide)
        {
            NewbieGuideStart();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isfinish == false)
        {
            if (currentState == State.Step2)
            {
                if (GameManager.instance.firePlaneManager.firePlanes[0].fireWork != null)
                {
                    if (GameManager.instance.firePlaneManager.firePlanes[0].fireWork.curFireworkLevel == 1)
                    {
                        newbie[1].SetActive(false);
                        newbie[0].SetActive(true);
                        currentState = State.Step3;
                    }
                }
                
            }else
            if (currentState == State.Step4)
            {
                if (GameManager.instance.preparePlaneManager.preparePlanes[1].fireWork != null)
                {
                    if (GameManager.instance.preparePlaneManager.preparePlanes[1].fireWork.curFireworkLevel == 2)
                    {
                        newbie[2].SetActive(false);
                        newbie[3].SetActive(true);
                        currentState = State.Step5;
                    }
                }
                
            }else
            if (currentState == State.Step5)
            {
                if (GameManager.instance.firePlaneManager.firePlanes[1].fireWork != null)
                {
                    if (GameManager.instance.firePlaneManager.firePlanes[1].fireWork.curFireworkLevel == 2)
                    {
                        newbie[3].SetActive(false);
                        isfinish = true;
                        PlayerPrefs.SetInt(G.NEWBIEGUIDE, 1);
                    }
                }
                
            }
        }
        
    }
    public void NewbieGuideStart()
    {
        newbie[0].SetActive(true);
        buyFireworkbutton.onClick.AddListener(() =>
        {
            clickCount++;
            if (clickCount == 1&& isfinishStep==false)
            {
                newbie[0].SetActive(false);
                newbie[1].SetActive(true);
                currentState = State.Step2;
                isfinishStep =true;
                
                
            }
            if (currentState == State.Step3)
            {
                clickCount1++;
                if (clickCount1 == 2 && isfinishStep1 == false)
                {
                    newbie[0].SetActive(false);
                    newbie[2].SetActive(true);
                    currentState = State.Step4;
                    isfinishStep1 = true;
                }
            }
        });
    }
   
}
