using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GamePhase
{
    Prepare, Start, Play, GameOver, DoNothing
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SettingsUI settingsUI;
    public PlayUI playUI;
    public ResultUI resultUI;

    [SerializeField]
    GameObject flyCoinPrefab;

    [HideInInspector]
    public GamePhase gp;
    

    UserInput userInput;
    LevelData curLevelData;
    int curLevel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        G.dc.Load();
        InitGameData();
        playUI.UpdateUI(curLevel);
        userInput = new UserInput(ControlStart, ControlMove, ControlStationary, ControlEnd);
        gp = GamePhase.Prepare;
    }

    void InitGameData()
    {
        curLevel = PlayerPrefs.GetInt(G.LEVEL, 1);
        curLevelData = G.dc.gd.GetLevelData(curLevel);
    }

    public LevelData CurrentLevelData()
    {
        return curLevelData;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gp)
        {
            case GamePhase.Start:
                break;
            case GamePhase.Play:
                break;
        }
    }


    void ControlStart(Vector2 pos)
    {
        if (gp == GamePhase.Start)
        {
        }
    }

    void ControlMove(Vector2 pos)
    {
        if(gp == GamePhase.Play)
        {
        }
    }

    void ControlStationary(Vector2 pos)
    {
        ControlMove(pos);
    }

    void ControlEnd(Vector2 pos)
    {
        if (gp == GamePhase.Play)
        {
           
        }
    }

    public void GameOver()
    {
        gp = GamePhase.GameOver;
    }

    public void AddMoney(int amount)
    {
        G.dc.AddMoney(amount);
        G.dc.Save();
        playUI.UpdateUI(curLevel);
    }

    void AddLevel()
    {
        PlayerPrefs.SetInt(G.LEVEL, curLevel + 1);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        AudioManager.instance?.Tap();
        AddLevel();
        ReloadLevel();
    }

    public void ShowSettings()
    {
        AudioManager.instance?.Tap();
        settingsUI.Show();
    }

    public void GoToShopScene()
    {
        AudioManager.instance?.Tap();
        SceneManager.LoadScene(Scenes.SHOP_SCENE);
    }

    public void ShowResult()
    {
        AudioManager.instance?.Tap();
        resultUI.ShowResult();
    }

    public void FlyCoins(int amount, int count, Vector3 startPos)
    {
        //AudioManager.instance?.PlaySfx(AudioManager.instance.bonusCoins, 0, 0.5f);
        Haptics.Feedback();
        StartCoroutine(FLyCoins_(amount, count, startPos));
    }

    IEnumerator FLyCoins_(int amount, int count, Vector3 startPos)
    {
        float timer = 0, duration = G.FLY_COINS_DURATION;
        Vector3 targetPos = playUI.CollectMoneyPosition();
        List<FlyCoin> flyCoins = new List<FlyCoin>();
        var canvas = GameObject.Find("Canvas");
        for (var i = 0; i < count; i++)
        {
            var fd = Instantiate(flyCoinPrefab, canvas.transform).GetComponent<FlyCoin>();
            fd.startPos = startPos + new Vector3(Random.Range(-fd.range, fd.range), Random.Range(-fd.range, fd.range));
            flyCoins.Add(fd);
        }


        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            foreach (var fd in flyCoins)
            {
                fd.transform.position = Vector3.Slerp(fd.startPos, targetPos, timer / duration);
            }

        }

        playUI.BlowCoinText();
        yield return new WaitForSeconds(0.1f);
        AddMoney(amount);

        foreach (var fd in flyCoins)
        {
            Haptics.Feedback();
            Destroy(fd.gameObject);
        }
    }
}
