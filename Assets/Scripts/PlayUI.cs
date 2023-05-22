using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField]
    Text tipText, coinText, levelText;
    [SerializeField]
    Image moneyIcon;
    [SerializeField]
    Button nextButton, startButton;
    [SerializeField]
    Animator coinTextAnim;

    public void UpdateUI(int curLevel = 1)
    {
        coinText.text = G.FormatNum(G.dc.GetMoney());
        levelText.text = "Level " + curLevel;
        Debug.Log(coinText.text);
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
