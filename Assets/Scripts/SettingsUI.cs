using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public GameObject panel;
    public Button audioSwitch, hapticsSwitch,privacyPolicy,userTerms;
    public Sprite siwtchON, siwtchOFF;
    bool audioOn, hapticsOn;

    private void Start()
    {
        audioOn = PlayerPrefs.GetInt(G.AUDIO, 1) == 1 ? true : false;
        hapticsOn = PlayerPrefs.GetInt(G.HAPTICS, 1) == 1 ? true : false;
        audioSwitch.onClick.AddListener(SwitchAudio);
        hapticsSwitch.onClick.AddListener(SwitchHaptics);
        privacyPolicy.onClick.AddListener(PrivacyPolicyClick);
        userTerms.onClick.AddListener(UserTermsClick);

    }
    public void Show()
    {
        this.panel.SetActive(true);
        InitSettingUI();
    }
    public void Hide()
    {
        this.panel.SetActive(false);
    }
    public void  InitSettingUI()
    {
        audioSwitch.GetComponent<Image>().sprite = audioOn == true ? siwtchON : siwtchOFF;
        hapticsSwitch.GetComponent<Image>().sprite = hapticsOn == true ? siwtchON : siwtchOFF;
        StartCoroutine(SwitchButtonMove(audioSwitch, audioOn));
        StartCoroutine(SwitchButtonMove(hapticsSwitch, hapticsOn));
    }
    public void SwitchAudio()
    {
        audioOn = audioOn == true ? false : true;
        PlayerPrefs.SetInt(G.AUDIO, audioOn == true?1:0);
        audioSwitch.GetComponent<Image>().sprite = audioOn == true ? siwtchON : siwtchOFF;
        StartCoroutine( SwitchButtonMove(audioSwitch, audioOn));
    }
    public void SwitchHaptics()
    {
        hapticsOn = hapticsOn == true ? false : true;
        PlayerPrefs.SetInt(G.AUDIO, hapticsOn == true ? 1 : 0);
        hapticsSwitch.GetComponent<Image>().sprite = hapticsOn == true ? siwtchON : siwtchOFF;
        StartCoroutine(SwitchButtonMove(hapticsSwitch, hapticsOn));

    }
    IEnumerator SwitchButtonMove(Button button,bool on)
    {
        button.enabled = false;
        RectTransform buttonRT = button.GetComponent<RectTransform>();
        Vector3 onPos = new Vector3(4,0,0), offPos = new Vector3(110, 0, 0);
        Vector3 originalPos = on == false ? onPos : offPos;
        Vector3 aimPos = on == true ? onPos : offPos;

        float time = 0.2f;
        float timeRation = 1 / time;
        while(time > 0)
        {
            time -= Time.deltaTime;
            buttonRT.anchoredPosition = Vector3.Lerp(originalPos, aimPos,1- time * timeRation);
            yield return null;
        }

        button.enabled = true;
    }
    public void PrivacyPolicyClick()
    {
        Application.OpenURL("https://www.hypercasualgogo.com/privacy");

    }
    public void UserTermsClick()
    {
        Application.OpenURL("https://www.hypercasualgogo.com/tos");
    }


}
