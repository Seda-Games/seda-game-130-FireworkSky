using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GamePhase
{
    Prepare, Start, Play, GameOver, DoNothing
}

public class GameManager : SingleInstance<GameManager>
{
    public static GameManager instance;

    public SettingsUI settingsUI;
    public PlayUI playUI;
    public ResultUI resultUI;
    public FireWorkMove fireWorkMove;
    [SerializeField]
    GameObject flyCoinPrefab;

    
    public GamePhase gp;

    [HideInInspector] public Vector2 mouseDownPos, mouseFromDownToNowPos, mouseLastPos, mouseUpPos;

    UserInput userInput;
    LevelData curLevelData;
    public int curLevel;

    [SerializeField] public int curMoney;
    [SerializeField] public int curSizeValue;
    public FireWorkManager fireWorkManager;
    public FirePlaneManager firePlaneManager;
    public PreparePlaneManager preparePlaneManager;
    public FireWork fireWork;
    public PlayGame playGame;
    Vector2 mouseOriginalPoint, mouseLastPoint;
    private Vector3 target;
    public Player player;
    bool onClick = false;
    bool is_element=false;
    public GameObject element;
    public GameObject element2;
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
        if (!PlayerPrefs.HasKey(G.FIRST_LOGIN_TIME_MONEY))
        {
            curLevel = PlayerPrefs.GetInt(G.LEVEL, 1);
            //PlayerPrefs.SetInt(G.MONEY, G.dc.gd.levelDict[curLevel].money);
            AddMoney(G.dc.gd.levelDict[curLevel].money);
            var now = System.DateTime.Now;
            PlayerPrefs.SetString(G.FIRST_LOGIN_TIME_MONEY, now.ToBinary().ToString());
        }
        
        playUI.UpdateUI(curLevel);
        
       
        playUI.GameStartUI();
        userInput = new UserInput(ControlStart, ControlMove, ControlStationary, ControlEnd);
        gp = GamePhase.Prepare;
        target = player.transform.position;
        preparePlaneManager.InitPrepareFirePlane();
        firePlaneManager.InitFirePlane();
    }

    public void InitGameData()
    {
        curLevel = PlayerPrefs.GetInt(G.LEVEL, 1);
        curLevelData = G.dc.gd.GetLevelData(curLevel);
    }

    public LevelData CurrentLevelData()
    {
        return curLevelData;
    }
    public void GameStartClick()
    {
        playUI.GameStartClick();
        gp = GamePhase.Play;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gp)
        {
            case GamePhase.Start:
                break;
            case GamePhase.Play:
                userInput.UserControl();
                break;
        }
    }


    void ControlStart(Vector2 pos)
    {
        mouseDownPos = pos;
        if (gp == GamePhase.Start)
        {

        }
        if (gp == GamePhase.Play)
        {
            //fireWorkManager.FireWorkStart(pos);
        }
         
    }

    void ControlMove(Vector2 pos)
    {
        //fireWorkManager.FireWorkMove(pos);
    }

    void ControlStationary(Vector2 pos)
    {
        ControlMove(pos);
    }

    void ControlEnd(Vector2 pos)
    {
        mouseLastPoint = Vector2.zero;
        if (gp == GamePhase.Play)
        {
            /*Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit,hit2;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag(Tag.FireWork))
                {
                    is_element = true;
                    element = hit.collider.gameObject;
                    Player player = element.GetComponent<Player>();
                    player.pp = PlayerPhase.Selected;
                }
            }
            if (is_element)
            {
                if (Physics.Raycast(ray, out hit2, Mathf.Infinity))
                {
                    if (hit2.transform.CompareTag(Tag.Plane))
                    {
                        playGame.allCub.Remove(element);
                        element2 = hit2.collider.gameObject;
                        element.transform.position = element2.transform.position + new Vector3(0, 0.01f, 0);
                        Player player = element.GetComponent<Player>();
                        player.pp = PlayerPhase.UnSelected;
                        player.PlayFx();
                        is_element = false;
                    }
                    if (hit2.transform.CompareTag(Tag.FireWork))
                    {
                        
                        Debug.Log("点击了方块");
                        
                    }
                    //playGame.cub.transform.position = element.transform.position + new Vector3(0, 0.01f, 0);
                    Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(element.transform.position);
                    Vector3 mousePos = new Vector3(pos.x, pos.y,targetScreenPos.z);
                    player.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
                }

            }*/
            //fireWorkManager.FireWorkMoveEnd(pos);
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

    public void UseMoney(int level)
    {
        G.dc.UseMoney(G.dc.gd.levelDict[level].fireworkcost);
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
    //移动方法
    void Move(Vector3 target)
    {
        if (Vector3.Distance(player.transform.position, target) > 0.1f)
        {
            player.transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
        }
        //如果物体的位置和目标点的位置距离小于 0.1时直接等于目标点
        else
            player.transform.position = target;
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
