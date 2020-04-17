using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip tap, pick, tab, ticking;
    [HideInInspector]
    public AudioSource srcBgm, srcSfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AudioSource[] ass = GetComponents<AudioSource>();
        srcBgm = ass[0];
        srcSfx = ass[1];
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySfx(AudioClip clip, float delay = 0, float volumn = 1f)
    {
        int audioOn = PlayerPrefs.GetInt(G.AUDIO, 1);
        if (audioOn == 0)
            return;

        srcSfx.clip = clip;
        srcSfx.volume = volumn;
        if (delay > 0)
        {
            srcSfx.PlayDelayed(delay);
        }
        else
        {
            srcSfx.PlayOneShot(clip);
        }
    }

    public void PlayBgm()
    {
        int audioOn = PlayerPrefs.GetInt(G.AUDIO, 1);
        if (audioOn == 0)
            srcBgm.Pause();
        else
        {
            //srcBgm.volume = volumn;
            srcBgm.Play();
        }
    }

    public void PauseBgm()
    {
        srcBgm.Pause();
    }

    public void Tap()
    {
        PlaySfx(tap);
    }

    public void Pick()
    {
        PlaySfx(pick);
    }

    public void Tick()
    {
        PlaySfx(ticking);
    }
}
