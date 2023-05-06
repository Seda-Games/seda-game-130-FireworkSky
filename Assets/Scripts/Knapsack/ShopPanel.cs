
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    public ButtonSwitch buttonSwitch;
    public Button unlockButton, getMoneyAdButton;
    public BoxMask boxMask;
    public Text moneyText;
    public ExhibitionManager exhibitionManager;
    private void Awake()
    {
        G.dc.Load();
    }
    void Start()
    {
        moneyText.text = G.FormatNum(G.dc.GetMoney());

    }

    public void InitShowPanel()
    {
        buttonSwitch.buttons[0].onClick.Invoke();
    }
    void Update()
    {
        
    }
    public  void RetruenButtonClick()
    {
        SceneManager.LoadScene(Scenes.PLAY_SCENE);
    }
    public void ShowBox(int index)
    {
        boxMask.ShowBox(index);
    }
    public void RandomUnlockSkin()
    {
        //随机解锁当前页面的可解锁物品
        boxMask.GetCurUIBox().RandomUnlockItem();
    }
    void AddMoney(int amount)
    {
        G.dc.AddMoney(amount);
        G.dc.Save();
        moneyText.text = G.FormatNum(G.dc.GetMoney());
    }
    public void ShowModel(string modelName)
    {
        string path = "Model/" + modelName;
        exhibitionManager.LoadModelWithEffect(path);
    }
}
