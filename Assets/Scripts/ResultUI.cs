using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    Button retryButton, continueButton;

    public void ShowResult()
    {
        gameObject.SetActive(true);
    }

    public void Retry()
    {
        AudioManager.instance?.Tap();
        GameManager.instance.ReloadLevel();
    }
}
