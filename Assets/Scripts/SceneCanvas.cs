using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class SceneCanvas : MonoBehaviour
{
    public GameObject moneyTextObj;

    GameObjectPool moneyTextObjPool;

    private void Awake()
    {
        moneyTextObjPool = new GameObjectPool(moneyTextObj);
        moneyTextObj.transform.forward = CameraManager.Instance.transform.forward;
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
}
