using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public Switch audioSwitch, hapticsSwitch;
    bool audioOn, hapticsOn;
    // Start is called before the first frame update
    void Start()
    {
        audioOn = PlayerPrefs.GetInt(G.AUDIO, 1) == 1;
        if (!audioOn)
            audioSwitch.Toggle();
        hapticsOn = PlayerPrefs.GetInt(G.HAPTICS, 1) == 1;
        if (!hapticsOn)
            hapticsSwitch.Toggle();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ToggleAudio()
    {
        audioOn = !audioOn;
        PlayerPrefs.SetInt(G.AUDIO, audioOn ? 1 : 0);
        //if (!audioOn)
        //    AudioManager.instance?.srcBgm.Pause();
        //else
        //    AudioManager.instance?.srcBgm.Play();
    }

    public void ToggleHaptics()
    {
        hapticsOn = !hapticsOn;
        PlayerPrefs.SetInt(G.HAPTICS, hapticsOn ? 1 : 0);
    }
}
