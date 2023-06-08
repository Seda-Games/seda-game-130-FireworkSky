using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HumanManager : MonoBehaviour
{
    public GameObject[] characterPrefab; // 人物预制体
    public Vector3[] spawnPoint; // 出生点
    public Vector3[] areaCenter; // 区域中心点
    public Vector3[] areaSize; // 区域大小
    public Vector3[] destroyPoint;//结束点
    public Transform watchPoint;
    float timeLine;
    public int visitorLevel=1;
    private int human = 0;
    private int stage1;
    private bool isachieve;
    private float duration1 = 0;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.GetInt(G.VISITOR, 1);
        human = PlayerPrefs.GetInt(G.ACHIEVEMENTHUMAN, 0);
        stage1 = PlayerPrefs.GetInt(G.ACHIEVEMENTHUMANSTAGE, 1);
        //StartCoroutine(Visitor());

    }

    // Update is called once per frame
    void Update()
    {
        timeLine += Time.deltaTime;
        //Debug.Log("hhahahahah" + PlayerPrefs.GetInt(G.VISITOR, visitorLevel));
        //fireWorkLevel = Mathf.Clamp(fireWorkLevel, G.dc.gd.addFireWorkDatas[0].level, G.dc.gd.addFireWorkDatas[G.dc.gd.addFireWorkDatas.Length - 1].level);
        visitorLevel = PlayerPrefs.GetInt(G.VISITOR, 2);
        visitorLevel = Mathf.Clamp(visitorLevel, G.dc.gd.humanDatas[0].level, G.dc.gd.humanDatas[G.dc.gd.humanDatas.Length - 1].level);
        if (timeLine > G.dc.gd.humanDataDataDict[visitorLevel].second)
        {
            
            int stage = PlayerPrefs.GetInt(G.STAGE, 1)-1;
            StartCoroutine(Visitor(stage));
            timeLine = 0;
        }
        if (isachieve == true)
        {
            duration1 += Time.deltaTime;
            Debug.Log("现在是" + duration1);

            int num = Mathf.Clamp(PlayerPrefs.GetInt(G.ACHIEVEMENTHUMANSTAGE, stage1), G.dc.gd.achievementTables[0].level, G.dc.gd.achievementTableDict[G.dc.gd.achievementTables.Length].level);
            if (duration1 > G.dc.gd.achievementTableDict[num].duration)
            {
                isachieve = false;
                duration1 = 0;
            }
        }
    }
    void OnDrawGizmos()
    {
        // 绘制长方形区域
        Gizmos.DrawWireCube(areaCenter[0], areaSize[0]);
        Gizmos.DrawWireCube(areaCenter[1], areaSize[1]);
        Gizmos.DrawWireCube(areaCenter[2], areaSize[2]);
    }
    IEnumerator Visitor(int stage)
    {
        
        for (int i = 0; i < G.dc.gd.humanDataDataDict[visitorLevel-1].flow; i++)
        {
            int number = Random.Range(0, characterPrefab.Length);
            // 在出生点生成一个人物
            GameObject characterObj = Instantiate(characterPrefab[number], spawnPoint[stage], Quaternion.identity);
            Animator animator = characterObj.GetComponent<Animator>();
            characterObj.GetComponent<Human>().curLevel = G.dc.gd.humanDataDataDict[visitorLevel - 1].level;
            characterObj.GetComponent<Human>().curIncome= G.dc.gd.humanDataDataDict[characterObj.GetComponent<Human>().curLevel].income;
            // 随机生成一个区域内的点
            Vector3 targetPoint = new Vector3(
                Random.Range(areaCenter[stage].x - areaSize[stage].x / 2, areaCenter[stage].x + areaSize[stage].x / 2),
                Random.Range(areaCenter[stage].y - areaSize[stage].y / 2, areaCenter[stage].y + areaSize[stage].y / 2),
                Random.Range(areaCenter[stage].z - areaSize[stage].z / 2, areaCenter[stage].z + areaSize[stage].z / 2)
            );
            float duration = Vector3.Distance(characterObj.transform.position, targetPoint) / 2f;

            human += 1;
            PlayerPrefs.SetInt(G.ACHIEVEMENTHUMAN, human);
            stage1 = Mathf.Clamp(PlayerPrefs.GetInt(G.ACHIEVEMENTHUMANSTAGE, stage1), G.dc.gd.achievementTables[0].level, G.dc.gd.achievementTableDict[G.dc.gd.achievementTables.Length].level);
            if (PlayerPrefs.GetInt(G.ACHIEVEMENTHUMAN, human) >= G.dc.gd.achievementTableDict[stage1].accumulatehuman)
            {
                stage1 += 1;
                PlayerPrefs.SetInt(G.ACHIEVEMENTHUMANSTAGE, stage1);
                isachieve = true;
                human = 0;
                PlayerPrefs.SetInt(G.ACHIEVEMENTHUMAN, human);
            }
            
            GameManager.instance.playUI.UpdateHumanNumber(PlayerPrefs.GetInt(G.ACHIEVEMENTHUMANSTAGE, stage1));


            // 使用 DOTween 让人物移动到目标点
            // 使用 DOTween 让人物始终面向移动方向
            characterObj.transform.DOLookAt(targetPoint, 0.1f);
            characterObj.transform.DOMove(targetPoint, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                animator.SetTrigger("WalkToIdle1");
                
                characterObj.transform.DORotate(watchPoint.eulerAngles, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Debug.Log("dwqdqwdqwdqwdqwdqd");
                    number = Random.Range(0, GameSceneManager.Instance.sceneCanvas.emojiObj.Length);
                    GameSceneManager.Instance.sceneCanvas.EmojiPool = new GameObjectPool(GameSceneManager.Instance.sceneCanvas.emojiObj[number]);
                    GameSceneManager.Instance.sceneCanvas.emojiObj[number].transform.forward = CameraManager.Instance.transform.forward;
                    GameSceneManager.Instance.sceneCanvas.ShowEmoji(characterObj.transform.position+new Vector3(0,0.2f,0));
                    // 让人物在目标点停留10秒
                    DOVirtual.DelayedCall(10f, () =>
                    {
                        if (isachieve == true)
                        {
                            GameManager.instance.AddMoney(characterObj.GetComponent<Human>().curIncome * 2);
                            GameSceneManager.Instance.sceneCanvas.ShowMoneyText(characterObj.transform.position + Vector3.up, characterObj.GetComponent<Human>().curIncome * 2);
                        }
                        else
                        if(isachieve == false)
                        {
                            GameManager.instance.AddMoney(characterObj.GetComponent<Human>().curIncome);
                            GameSceneManager.Instance.sceneCanvas.ShowMoneyText(characterObj.transform.position + Vector3.up, characterObj.GetComponent<Human>().curIncome);
                        }
                        animator.SetTrigger("Walk");
                        // 使用 DOTween 让人物始终面向移动方向
                        characterObj.transform.DOLookAt(destroyPoint[0], 0.1f).OnComplete(() =>
                        {
                            // 计算移动时间
                            float duration = Vector3.Distance(characterObj.transform.position, destroyPoint[0]) / 2f;
                            // 使用 DOTween 让人物移动到销毁点
                            characterObj.transform.DOMove(destroyPoint[0], duration).SetEase(Ease.Linear).OnComplete(() =>
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
        visitorLevel=PlayerPrefs.GetInt(G.VISITOR, 2);
        visitorLevel = Mathf.Clamp(visitorLevel, G.dc.gd.humanDatas[0].level, G.dc.gd.humanDataDataDict[G.dc.gd.humanDatas.Length - 1].level);
        if (G.dc.GetMoney() >= G.dc.gd.humanDataDataDict[visitorLevel].cost)
        {
            GameManager.instance.UseHumanMoney(visitorLevel);
            visitorLevel += 1;
            PlayerPrefs.SetInt(G.VISITOR, visitorLevel);
            if (visitorLevel == G.dc.gd.humanDatas.Length + 1)
            {
                GameManager.instance.bottomPanel.maxaddcrowd.SetActive(true);
                GameManager.instance.bottomPanel.visitorButton.interactable = false;
                GameManager.instance.bottomPanel.addCrowdText.SetActive(false);
                GameManager.instance.ismax1 = true;
            }
            GameManager.instance.IsEnoughMoney();
            Debug.Log(PlayerPrefs.GetInt(G.VISITOR, visitorLevel));
            
        }
        else
        {
            Debug.LogError("钱不够，无法继续升级");
        }


    }
    //G.dc.gd.fireWorkDataDict[level].income
}
