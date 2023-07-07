using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prepare : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirstLoginMark();
        LoginMark();
        SDKManager.InitAdUnitySDK();
        SceneManager.LoadScene(Scenes.PLAY_SCENE);
    }

    void FirstLoginMark()
    {
        if (!PlayerPrefs.HasKey(G.FIRST_LOGIN_TIME))
        {
            var now = System.DateTime.Now;
            PlayerPrefs.SetString(G.FIRST_LOGIN_TIME, now.ToBinary().ToString());
            print("Saving first login time:" + now);
        }
    }

    void LoginMark()
    {
        var now = System.DateTime.Now;
        PlayerPrefs.SetString(G.LOGIN_TIME, now.ToBinary().ToString());
        print("Saving login time:" + now);
    }
}