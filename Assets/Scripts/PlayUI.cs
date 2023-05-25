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

    public void UpdateUI(int curLevel = 1)
    {
        coinText.text = "$"+G.FormatNum(G.dc.GetMoney());
        levelText.text = "Level " + curLevel;
       


    }
    public void InitText()
    {
        AddFireWorkText.text = G.FormatNum(G.dc.gd.addFireWorkDataDict[G.dc.GetNextCost() + 1].cost);
        AddHumanText.text = G.FormatNum(G.dc.gd.humanDataDataDict[G.dc.GetNextHumanCost() + 1].cost);
        AddIncomeText.text = G.FormatNum(G.dc.gd.AddIncomeDataDict[G.dc.GetNextIncomeCost() + 1].cost);
    }
    public void UpdateLevelUI(int level)
    {
        coinText.text = "$"+G.FormatNum(G.dc.GetMoney());
        level = Mathf.Clamp(GameManager.instance.fireWorkManager.fireWorkLevel + 1, G.dc.gd.addFireWorkDatas[0].level, G.dc.gd.addFireWorkDataDict[G.dc.gd.addFireWorkDatas.Length - 1].level);
        AddFireWorkText.text = G.FormatNum(G.dc.gd.addFireWorkDataDict[level].cost);
    }
    public void UpdateLevelHumanUI(int level)
    {
        coinText.text = "$"+ G.FormatNum(G.dc.GetMoney());
        humanText.text = G.FormatNum(G.dc.gd.humanDataDataDict[level].flow);
        Debug.Log("客流量" + G.FormatNum(G.dc.gd.humanDataDataDict[level].flow));
        level = Mathf.Clamp(GameManager.instance.humanManager.visitorLevel + 1, G.dc.gd.humanDatas[0].level, G.dc.gd.humanDataDataDict[G.dc.gd.humanDatas.Length - 1].level);
        AddHumanText.text = G.FormatNum(G.dc.gd.humanDataDataDict[level].cost);
        
       
    }
    public void UpdateLevelIncomeUI(int level)
    {
        
        coinText.text = "$"+ G.FormatNum(G.dc.GetMoney());
        level=Mathf.Clamp(GameManager.instance.fireWorkManager.addIncomelevel + 1, G.dc.gd.addIncomeDatas[0].level, G.dc.gd.AddIncomeDataDict[G.dc.gd.addIncomeDatas.Length - 1].level);
        AddIncomeText.text= G.FormatNum(G.dc.gd.AddIncomeDataDict[level].cost);
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
        int amount = Random.Range(50, 200);
        GameManager.instance.FlyCoins(amount, amount / 10, transform.position);
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
