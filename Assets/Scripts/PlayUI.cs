using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField]
    Text tipText, coinText, levelText,humanText;
    [SerializeField]
    Image moneyIcon;
    [SerializeField]
    Button nextButton, startButton;
    [SerializeField]
    Animator coinTextAnim;

    public Text AddFireWorkText;
    public Text AddHumanText;
    public Text AddIncomeText;

    public int fireworkLevel;
    public int addhumanLevel;
    public int addincomeLevel;

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
        

        humanText.text = G.FormatNum1((G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, 1)].flow / G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, 1)].second) * 60) + "/min";
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
        int amount = Random.Range(50000, 100000);
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
}
