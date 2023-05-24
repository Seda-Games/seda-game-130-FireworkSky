using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HumanManager : MonoBehaviour
{
    public GameObject characterPrefab; // 人物预制体
    public Vector3 spawnPoint; // 出生点
    public Vector3 areaCenter; // 区域中心点
    public Vector3 areaSize; // 区域大小
    public Vector3 destroyPoint;//结束点
    public Transform watchPoint;
    float timeLine;
    public int visitorLevel=1;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.GetInt(G.VISITOR, 1);
        //StartCoroutine(Visitor());

    }

    // Update is called once per frame
    void Update()
    {
        timeLine += Time.deltaTime;
        //Debug.Log("hhahahahah" + PlayerPrefs.GetInt(G.VISITOR, visitorLevel));
        if (timeLine > G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, visitorLevel)].second)
        {
            Debug.Log(PlayerPrefs.GetInt(G.VISITOR,1));
            StartCoroutine(Visitor());
            timeLine = 0;
        }
    }
    void OnDrawGizmos()
    {
        // 绘制长方形区域
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
    IEnumerator Visitor()
    {
        for (int i = 0; i < G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, 1)].flow; i++)
        {
            // 在出生点生成一个人物
            GameObject characterObj = Instantiate(characterPrefab, spawnPoint, Quaternion.identity);
            Animator animator = characterObj.GetComponent<Animator>();
            characterObj.GetComponent<Human>().curLevel = G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, visitorLevel)].level;
            characterObj.GetComponent<Human>().curIncome= G.dc.gd.humanDataDataDict[characterObj.GetComponent<Human>().curLevel].income;
            // 随机生成一个区域内的点
            Vector3 targetPoint = new Vector3(
                Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2),
                Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2),
                Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2)
            );
            float duration = Vector3.Distance(characterObj.transform.position, targetPoint) / 2f;
            // 使用 DOTween 让人物移动到目标点
            // 使用 DOTween 让人物始终面向移动方向
            characterObj.transform.DOLookAt(targetPoint, 0.1f);
            characterObj.transform.DOMove(targetPoint, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                animator.SetTrigger("WalkToIdle1");
                
                characterObj.transform.DORotate(watchPoint.eulerAngles, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Debug.Log("dwqdqwdqwdqwdqwdqd");
                    // 让人物在目标点停留10秒
                    DOVirtual.DelayedCall(10f, () =>
                    {
                        GameManager.instance.AddMoney(characterObj.GetComponent<Human>().curIncome);
                        GameSceneManager.Instance.sceneCanvas.ShowMoneyText(characterObj.transform.position+ Vector3.up, characterObj.GetComponent<Human>().curIncome);
                        animator.SetTrigger("Walk");
                        // 使用 DOTween 让人物始终面向移动方向
                        characterObj.transform.DOLookAt(destroyPoint, 0.1f).OnComplete(() =>
                        {
                            // 计算移动时间
                            float duration = Vector3.Distance(characterObj.transform.position, destroyPoint) / 2f;
                            // 使用 DOTween 让人物移动到销毁点
                            characterObj.transform.DOMove(destroyPoint, duration).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                // 销毁人物
                                Destroy(characterObj);
                            });
                        });

                    });
                });
            });
            yield return new WaitForSeconds(1f);
        }
        
    }
   
    public void AddVisitor()
    {
        Debug.Log(visitorLevel);
        Debug.Log(G.dc.gd.humanDataDataDict[visitorLevel].cost);
        if (G.dc.GetMoney() >= G.dc.gd.humanDataDataDict[visitorLevel].cost)
        {
            visitorLevel += 1;
            PlayerPrefs.SetInt(G.VISITOR, visitorLevel);
            GameManager.instance.UseHumanMoney(PlayerPrefs.GetInt(G.VISITOR, visitorLevel));
            Debug.Log(PlayerPrefs.GetInt(G.VISITOR, visitorLevel));
        }
        else
        {
            Debug.LogError("钱不够，无法继续升级");
        }


    }
    //G.dc.gd.fireWorkDataDict[level].income
}
