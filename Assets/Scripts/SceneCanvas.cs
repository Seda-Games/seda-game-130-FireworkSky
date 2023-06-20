using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class SceneCanvas : MonoBehaviour
{
    public GameObject moneyTextObj;
    public GameObject upgradeFxObj;
    public GameObject[] emojiObj;
    public GameObject sademojiObj;
    public GameObject happyemojiObj;
    GameObjectPool moneyTextObjPool;
    GameObjectPool UpgradeFxPool;
    public GameObjectPool EmojiPool;
    public GameObjectPool SadEmojiPool;
    public GameObjectPool HappyEmojiPool;
    public int number;
    public int number1;
    public int number2;
    private void Awake()
    {
        moneyTextObjPool = new GameObjectPool(moneyTextObj);
        moneyTextObj.transform.forward = CameraManager.Instance.transform.forward;
        UpgradeFxPool = new GameObjectPool(upgradeFxObj);
        //EmojiPool = new GameObjectPool(emojiObj[0]);
        SadEmojiPool= new GameObjectPool(sademojiObj);
        sademojiObj.transform.forward = CameraManager.Instance.transform.forward;
        
        HappyEmojiPool = new GameObjectPool(happyemojiObj);
        happyemojiObj.transform.forward= CameraManager.Instance.transform.forward;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowMoneyText(Vector3 pos, int money)
    {
        GameObject moneyText = moneyTextObjPool.GetGameObject(moneyTextObj.transform.parent);
        moneyText.transform.position = pos;
        moneyText.GetComponent<Text>().text = "+" + G.FormatNum(money);
        StartCoroutine(MoneyText(moneyTextObjPool, moneyText, 1.6f));
    }
    IEnumerator MoneyText(GameObjectPool objPool, GameObject obj, float time)
    {
        obj.transform.localScale = Vector3.one * 0.01f;
        obj.transform.DOMove(obj.transform.position + Vector3.up * 3, time * 0.7f);
        obj.transform.DOScale(Vector3.one * 0.014f, time * 0.5f);
        yield return new WaitForSeconds(time);
        objPool.RemoveGameObject(obj);
    }
    public void ShowUpgradeFx(Vector3 pos)
    {
        GameObject fx = UpgradeFxPool.GetGameObject(upgradeFxObj.transform.parent);
        fx.transform.position = pos;
        StartCoroutine(ShowFx(UpgradeFxPool, fx, 1f));
    }
    IEnumerator ShowFx(GameObjectPool objPool, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        objPool.RemoveGameObject(obj);
    }

    public void ShowEmoji(Vector3 pos)
    {
        GameObject emoji = EmojiPool.GetGameObject(emojiObj[number].transform.parent);
        emoji.transform.position = pos+new Vector3(0,2,0);
        StartCoroutine(ShowEmo(EmojiPool, emoji, 1.6f));
    }
    public void UnShowEmoji(Vector3 pos)
    {
        GameObject emoji = SadEmojiPool.GetGameObject(sademojiObj.transform.parent);
        emoji.transform.position = pos + new Vector3(0, 2, 0);
        StartCoroutine(SadShowEmo(SadEmojiPool, emoji, 1.6f));
    }

    public void HappuShowEmoji(Vector3 pos)
    {
        GameObject emoji = HappyEmojiPool.GetGameObject(happyemojiObj.transform.parent);
        emoji.transform.position = pos + new Vector3(0, 2, 0);
        StartCoroutine(HappyShowEmo(HappyEmojiPool, emoji, 1.6f));
    }
    IEnumerator ShowEmo(GameObjectPool objPool, GameObject obj, float time)
    {
        obj.transform.localScale = Vector3.one * 1f;
        obj.transform.DOMove(obj.transform.position + Vector3.up * 0.5f, time * 0.7f);
        //obj.transform.DOScale(Vector3.one * 0.014f, time * 0.5f);
        yield return new WaitForSeconds(time);
        objPool.RemoveGameObject(obj);
        Destroy(obj);
    }
    IEnumerator SadShowEmo(GameObjectPool objPool, GameObject obj, float time)
    {
        obj.transform.localScale = Vector3.one * 1f;
        obj.transform.DOMove(obj.transform.position + Vector3.up * 0.5f, time * 0.7f);
        //obj.transform.DOScale(Vector3.one * 0.014f, time * 0.5f);
        yield return new WaitForSeconds(time);
        objPool.RemoveGameObject(obj);
    }
    IEnumerator HappyShowEmo(GameObjectPool objPool, GameObject obj, float time)
    {
        obj.transform.localScale = Vector3.one * 1f;
        obj.transform.DOMove(obj.transform.position , time * 0.7f);
        //obj.transform.DOScale(Vector3.one * 0.014f, time * 0.5f);
        yield return new WaitForSeconds(time);
        objPool.RemoveGameObject(obj);
    }
}
