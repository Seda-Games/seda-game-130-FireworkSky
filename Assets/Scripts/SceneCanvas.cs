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
    GameObjectPool moneyTextObjPool;
    GameObjectPool UpgradeFxPool;
    GameObjectPool EmojiPool;
    public int number;
    private void Awake()
    {
        moneyTextObjPool = new GameObjectPool(moneyTextObj);
        moneyTextObj.transform.forward = CameraManager.Instance.transform.forward;
        UpgradeFxPool = new GameObjectPool(upgradeFxObj);
        number = Random.Range(0, emojiObj.Length);
        EmojiPool = new GameObjectPool(emojiObj[number]);
        emojiObj[number].transform.forward = CameraManager.Instance.transform.forward;
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
        moneyText.GetComponent<Text>().text = "+" + money;
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
        emoji.transform.position = pos;
        StartCoroutine(ShowEmo(EmojiPool, emoji, 1.6f));
    }
    IEnumerator ShowEmo(GameObjectPool objPool, GameObject obj, float time)
    {
        obj.transform.localScale = Vector3.one * 1f;
        obj.transform.DOMove(obj.transform.position + Vector3.up * 2.5f, time * 0.7f);
        //obj.transform.DOScale(Vector3.one * 0.014f, time * 0.5f);
        yield return new WaitForSeconds(time);
        objPool.RemoveGameObject(obj);
    }
}
