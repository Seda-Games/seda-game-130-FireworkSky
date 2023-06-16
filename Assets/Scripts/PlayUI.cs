using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField]
    Text tipText, coinText, levelText,humanText,numberText, humanNumberText,ratenumberText,ratehumanText;
    [SerializeField]
    Image moneyIcon;
    [SerializeField]
    Button nextButton, startButton;
    [SerializeField]
    Animator coinTextAnim;
    [SerializeField]
    Image launch, human;
    [SerializeField]
    Button launchButton, humanButton;
    [SerializeField]
    GameObject launchUI, humanUI;
    public Button launchRewardButton, huamanRewardButton, UnRewardLaunch, UnRewardHuman;

    public Text AddFireWorkText;
    public Text AddHumanText;
    public Text AddIncomeText;

    [SerializeField]
    Text fireworkMultiple, humanMultiple, fireworkMultipleUI, humanMultipleUI;


    public int fireworkLevel;
    public int addhumanLevel;
    public int addincomeLevel;

    public bool isreward = false;
    public bool isrewardhuman = false;

    [SerializeField]
    Button newfireworkUI;

    public GameObject fireworkUI;

    public Button nextmapButton;

    public GameObject nextmap;
    public GameObject unnextmap;

    public Text nextmapcost;
    public Text unnextmapcost;


    private void Start()
    {
        launchButton.onClick.AddListener(ShowLaunchUI);
        humanButton.onClick.AddListener(ShowHumanUI);

        UnRewardLaunch.onClick.AddListener(HideLaunchUI);
        UnRewardHuman.onClick.AddListener(HideHumanUI);

        launchRewardButton.onClick.AddListener(RewardLaunchUI);
        huamanRewardButton.onClick.AddListener(RewardHumanUI);

        newfireworkUI.onClick.AddListener(HideFireworkUI);

        nextmapButton.onClick.AddListener(NextMap);
    }
    public void UpdateUI(int curLevel = 1)
    {
        coinText.text = "$"+G.FormatNum(G.dc.GetMoney());
        levelText.text = "Level " + curLevel;
       


    }
    public void InitText()
    {
        fireworkLevel = PlayerPrefs.GetInt(G.FIREWORKLEVEL, 2);
        fireworkLevel = Mathf.Clamp(fireworkLevel, G.dc.gd.addFireWorkDatas[0].level, G.dc.gd.addFireWorkDatas[G.dc.gd.addFireWorkDatas.Length - 1].level);
        AddFireWorkText.text = G.FormatNum(G.dc.gd.addFireWorkDataDict[fireworkLevel].cost);
        
        addhumanLevel= PlayerPrefs.GetInt(G.VISITOR, 2);
        addhumanLevel = Mathf.Clamp(addhumanLevel, G.dc.gd.humanDatas[0].level, G.dc.gd.humanDatas[G.dc.gd.humanDatas.Length - 1].level);
        AddHumanText.text = G.FormatNum(G.dc.gd.humanDataDataDict[addhumanLevel].cost);

        addincomeLevel= PlayerPrefs.GetInt(G.UNLOCK, 2);
        addincomeLevel = Mathf.Clamp(addincomeLevel, G.dc.gd.firworkPlaneTables[0].level, G.dc.gd.firworkPlaneTables[G.dc.gd.firworkPlaneTables.Length - 1].level);
        AddIncomeText.text = G.FormatNum(G.dc.gd.firworkPlaneTableDict[addincomeLevel].unlockcost);
        if (PlayerPrefs.GetInt(G.UNLOCK, 2) == G.dc.gd.firworkPlaneTables.Length+1)
        {
            GameManager.instance.bottomPanel.IncomeButton.interactable = false;
            ColorBlock colors = GameManager.instance.bottomPanel.IncomeButton.colors;
            colors.disabledColor = Color.gray;
            GameManager.instance.bottomPanel.IncomeButton.colors = colors;
        }

        UpdateLauncherNumber(PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1));
        UpdateHumanNumber(PlayerPrefs.GetInt(G.ACHIEVEMENTHUMANSTAGE, 1));

        humanText.text = G.FormatNum1((G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, 1)].flow / G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, 1)].second) * 60) + "/min";

        UpdateNextmap(PlayerPrefs.GetInt(G.MAP, 1));
    }
    public void UpdateLevelUI(int level)
    {
        coinText.text = "$"+G.FormatNum(G.dc.GetMoney());
        level = Mathf.Clamp(level+1, G.dc.gd.addFireWorkDatas[0].level, G.dc.gd.addFireWorkDataDict[G.dc.gd.addFireWorkDatas.Length].level);
        AddFireWorkText.text = G.FormatNum(G.dc.gd.addFireWorkDataDict[level].cost);
    }
    public void UpdateLevelHumanUI(int level)
    {
        coinText.text = "$"+ G.FormatNum(G.dc.GetMoney());
        humanText.text = G.FormatNum1((G.dc.gd.humanDataDataDict[level].flow / G.dc.gd.humanDataDataDict[level].second)*60)+"/min";
        //humanText.text = G.FormatNum(G.dc.gd.humanDataDataDict[level].flow);
        //Debug.Log("客流量" + G.FormatNum1(G.dc.gd.humanDataDataDict[level].flow));
        level = Mathf.Clamp(level+1, G.dc.gd.humanDatas[0].level, G.dc.gd.humanDataDataDict[G.dc.gd.humanDatas.Length].level);
        AddHumanText.text = G.FormatNum(G.dc.gd.humanDataDataDict[level].cost);
        
       
    }
    public void UpdateLevelIncomeUI(int level)
    {
        
        coinText.text = "$"+ G.FormatNum(G.dc.GetMoney());
        //level=Mathf.Clamp(GameManager.instance.fireWorkManager.addIncomelevel + 1, G.dc.gd.addIncomeDatas[0].level, G.dc.gd.AddIncomeDataDict[G.dc.gd.addIncomeDatas.Length - 1].level);
        AddIncomeText.text= G.FormatNum(G.dc.gd.AddIncomeDataDict[level].cost);
        //Debug.Log("weishenmebukouqian"+G.FormatNum(G.dc.gd.AddIncomeDataDict[level].cost));
    }

    public void UpdateLevelUnlockFirePlaneUI(int level)
    {

        coinText.text = "$" + G.FormatNum(G.dc.GetMoney());
        level = Mathf.Clamp(level + 1, G.dc.gd.firworkPlaneTables[0].level, G.dc.gd.firworkPlaneTableDict[G.dc.gd.firworkPlaneTables.Length].level);
        //level=Mathf.Clamp(GameManager.instance.fireWorkManager.addIncomelevel + 1, G.dc.gd.addIncomeDatas[0].level, G.dc.gd.AddIncomeDataDict[G.dc.gd.addIncomeDatas.Length - 1].level);
        AddIncomeText.text = G.FormatNum(G.dc.gd.firworkPlaneTableDict[level].unlockcost);
        //Debug.Log("weishenmebukouqian"+G.FormatNum(G.dc.gd.AddIncomeDataDict[level].cost));
    }
    public void UpdateUnlockPreparePlaneUI()
    {
        coinText.text = "$" + G.FormatNum(G.dc.GetMoney());
    }
    public void UpdateLauncherNumber(int level)
    {
        if (level == G.dc.gd.achievementTables.Length + 1)
        {
            numberText.text = "MAX";
            launch.fillAmount = 1;
            ratenumberText.text = "MAX";
            launchRewardButton.gameObject.SetActive(false);
            UnRewardLaunch.gameObject.SetActive(true);
        }
        else
        {

            numberText.text = G.FormatNum(PlayerPrefs.GetInt(G.ACHIEVEMENT, 0)) + "/" + G.FormatNum(G.dc.gd.achievementTableDict[level].accumulatelauncher);
            launch.fillAmount = (float)PlayerPrefs.GetInt(G.ACHIEVEMENT, 0) / G.dc.gd.achievementTableDict[level].accumulatelauncher;


            if (GameManager.instance.fireWorkManager.isfinish == true)
            {
                ratenumberText.text = G.FormatNum(100) + "%";
            }
            else
            {
                float num = Mathf.Round(((float)PlayerPrefs.GetInt(G.ACHIEVEMENT, 0) / G.dc.gd.achievementTableDict[level].accumulatelauncher) * 100);
                ratenumberText.text = G.FormatNum1(num) + "%";
            }
        }
        
       

        //Debug.Log("现" + PlayerPrefs.GetInt(G.ACHIEVEMENT, 0));
        level = Mathf.Clamp(level, G.dc.gd.achievementTables[0].level, G.dc.gd.achievementTables[G.dc.gd.achievementTables.Length - 1].level);
        fireworkMultiple.text = "X" + G.FormatNum(G.dc.gd.achievementTableDict[level].multiple);
        fireworkMultipleUI.text= "X" + G.FormatNum(G.dc.gd.achievementTableDict[level].multiple);
        
        
        
        Debug.Log("现在是多少火箭" + launch.fillAmount);
    }
    public void UpdateHumanNumber(int level)
    {
        
        if (level == G.dc.gd.achievementTables.Length + 1)
        {
            humanNumberText.text = "MAX";
            human.fillAmount = 1;
            ratehumanText.text = "MAX";
            huamanRewardButton.gameObject.SetActive(false);
            UnRewardHuman.gameObject.SetActive(true);
        }
        else
        {
            humanNumberText.text = G.FormatNum(PlayerPrefs.GetInt(G.ACHIEVEMENTHUMAN, 0)) + "/" + G.FormatNum(G.dc.gd.achievementTableDict[level].accumulatehuman);
            human.fillAmount = (float)PlayerPrefs.GetInt(G.ACHIEVEMENTHUMAN, 0) / G.dc.gd.achievementTableDict[level].accumulatehuman;
            if (GameManager.instance.humanManager.isfinish == true)
            {
                ratehumanText.text = G.FormatNum(100) + "%";
            }
            else
            {
                float num = Mathf.Round(((float)PlayerPrefs.GetInt(G.ACHIEVEMENTHUMAN, 0) / G.dc.gd.achievementTableDict[level].accumulatehuman) * 100);
                ratehumanText.text = G.FormatNum1(num) + "%";
            }
        }

        level = Mathf.Clamp(level, G.dc.gd.achievementTables[0].level, G.dc.gd.achievementTables[G.dc.gd.achievementTables.Length - 1].level);
        humanMultiple.text = "X"+G.FormatNum(G.dc.gd.achievementTableDict[level].multiple);
        humanMultipleUI.text= "X"+G.FormatNum(G.dc.gd.achievementTableDict[level].multiple);
        
            
        Debug.Log("现在是多少" + human.fillAmount);
    }
    public void UpdateNextmap(int map)
    {
        if (map < G.dc.gd.levels.Length)
        {
            if (PlayerPrefs.GetInt(G.FIREWORKTIMES, 0) >= G.dc.gd.levelDict[map + 1].showlevel)
            {
                if (G.dc.GetMoney() >= G.dc.gd.levelDict[map + 1].nextmapcost)
                {
                    nextmap.SetActive(true);
                    nextmapcost.text = G.FormatNum(G.dc.gd.levelDict[map + 1].nextmapcost);
                }
                else
                {
                    unnextmapcost.text = G.FormatNum(G.dc.gd.levelDict[map + 1].nextmapcost);
                    nextmap.SetActive(false);
                    unnextmap.SetActive(true);
                }
            }
            else
            {
                nextmap.SetActive(false);
                unnextmap.SetActive(false);
            }
            if (PlayerPrefs.GetInt(G.FIREWORKTIMES, 0) >= G.dc.gd.levelDict[map].limitmaxlevel)
            {
                GameManager.instance.bottomPanel.needUnlockMap.SetActive(true);
            }
            else
            {
                GameManager.instance.bottomPanel.needUnlockMap.SetActive(false);
            }
        }
        else
        {
            nextmap.SetActive(false);
            unnextmap.SetActive(false);
        }
        
        
    }
    public void MoneyUI(int curLevel)
    {
        coinText.text = G.FormatNum(PlayerPrefs.GetInt(G.MONEY, G.dc.gd.levelDict[curLevel].money));
    }

    public Vector3 CollectMoneyPosition()
    {
        return moneyIcon.transform.position;
    }
    public void GameStartUI()
    {
        SetStartButton(true);
    }
    public void GameStartClick()
    {
        SetStartButton(false);
    }
    public void SetStartButton(bool isTure)
    {
        startButton.gameObject.SetActive(isTure);
    }

    public void ShowTip(string v = null)
    {
        if (v != null)
            tipText.text = v;
        tipText.gameObject.SetActive(true);
    }

    public void HideTip()
    {
        tipText.gameObject.SetActive(false);
    }

    public void ShowNextButton()
    {
        nextButton.gameObject.SetActive(true);
    }

    public void HideNextButton()
    {
        nextButton.gameObject.SetActive(false);
    }

    public void BlowCoinText()
    {
        if (!coinTextAnim.enabled)
            coinTextAnim.enabled = true;
        else
            coinTextAnim.SetTrigger("Blow");
    }

    public  void TestFlyMoney()
    {
        AudioManager.instance?.Tap();
        int amount = UnityEngine.Random.Range(50000, 100000);
        GameManager.instance.FlyCoins(amount, amount / 10000, transform.position);
    }

    public void TestToast()
    {
        AudioManager.instance?.Tap();
        ToastManager.Show("This is a test toast!");
    }

    public void TestHapticsFeedback(float intensity)
    {
        Haptics.Feedback(intensity);
    }
    public void ShowLaunchUI()
    {
        launchUI.SetActive(true);
    }
    public void ShowHumanUI()
    {
        humanUI.SetActive(true);
    }
    public void HideLaunchUI()
    {
        launchUI.SetActive(false);
    }
    public void HideHumanUI()
    {
        humanUI.SetActive(false);
    }
    public void RewardLaunchUI()
    {
        launchUI.SetActive(false);
        isreward = true;
        //GameManager.instance.fireWorkManager.isfinish = false;
        UpdateLauncherNumber(PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1));
        
        
    }
    public void RewardHumanUI()
    {
        humanUI.SetActive(false);
        isrewardhuman = true;
        //GameManager.instance.humanManager.isfinish = false;
        UpdateHumanNumber(PlayerPrefs.GetInt(G.ACHIEVEMENTHUMANSTAGE, 1));
    }
    public void HideFireworkUI()
    {
        fireworkUI.SetActive(false);
    }
    public void NextMap()
    {
        int map =PlayerPrefs.GetInt(G.MAP, 1);
        PlayerPrefs.SetInt(G.MAP, map + 1);
        
        GameManager.instance.map.ShowModel(G.dc.gd.levelDict[PlayerPrefs.GetInt(G.MAP, 1)].mapId);
        UpdateNextmap(PlayerPrefs.GetInt(G.MAP, 1));
        GameManager.instance.preparePlaneManager.InitNextMapPrepareFirePlane();
        GameManager.instance.firePlaneManager.InitNextMapFirePlane();
        GameManager.instance.buttonFireworkUI.ShowButtonUI(G.dc.gd.levelDict[PlayerPrefs.GetInt(G.MAP, 1)].firelevel);

    }
    public void InitModel()
    {
        GameManager.instance.map.ShowModel(G.dc.gd.levelDict[PlayerPrefs.GetInt(G.MAP, 1)].mapId);
    }
}
