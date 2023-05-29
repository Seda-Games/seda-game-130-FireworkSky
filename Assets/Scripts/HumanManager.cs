using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HumanManager : MonoBehaviour
{
    public GameObject[] characterPrefab; // ����Ԥ����
    public Vector3[] spawnPoint; // ������
    public Vector3[] areaCenter; // �������ĵ�
    public Vector3[] areaSize; // �����С
    public Vector3[] destroyPoint;//������
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
        //fireWorkLevel = Mathf.Clamp(fireWorkLevel, G.dc.gd.addFireWorkDatas[0].level, G.dc.gd.addFireWorkDatas[G.dc.gd.addFireWorkDatas.Length - 1].level);
        if (timeLine > G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, visitorLevel)].second)
        {
            Debug.Log(PlayerPrefs.GetInt(G.VISITOR,1));
            int stage = PlayerPrefs.GetInt(G.STAGE, 1)-1;
            StartCoroutine(Visitor(stage));
            timeLine = 0;
        }
    }
    void OnDrawGizmos()
    {
        // ���Ƴ���������
        Gizmos.DrawWireCube(areaCenter[0], areaSize[0]);
        Gizmos.DrawWireCube(areaCenter[1], areaSize[1]);
        Gizmos.DrawWireCube(areaCenter[2], areaSize[2]);
    }
    IEnumerator Visitor(int stage)
    {
        for (int i = 0; i < G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, 1)].flow; i++)
        {
            int number = Random.Range(0, characterPrefab.Length);
            // �ڳ���������һ������
            GameObject characterObj = Instantiate(characterPrefab[number], spawnPoint[stage], Quaternion.identity);
            Animator animator = characterObj.GetComponent<Animator>();
            characterObj.GetComponent<Human>().curLevel = G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, visitorLevel)].level;
            characterObj.GetComponent<Human>().curIncome= G.dc.gd.humanDataDataDict[characterObj.GetComponent<Human>().curLevel].income;
            // �������һ�������ڵĵ�
            Vector3 targetPoint = new Vector3(
                Random.Range(areaCenter[stage].x - areaSize[stage].x / 2, areaCenter[stage].x + areaSize[stage].x / 2),
                Random.Range(areaCenter[stage].y - areaSize[stage].y / 2, areaCenter[stage].y + areaSize[stage].y / 2),
                Random.Range(areaCenter[stage].z - areaSize[stage].z / 2, areaCenter[stage].z + areaSize[stage].z / 2)
            );
            float duration = Vector3.Distance(characterObj.transform.position, targetPoint) / 2f;
            // ʹ�� DOTween �������ƶ���Ŀ���
            // ʹ�� DOTween ������ʼ�������ƶ�����
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
                    // ��������Ŀ���ͣ��10��
                    DOVirtual.DelayedCall(10f, () =>
                    {
                        GameManager.instance.AddMoney(characterObj.GetComponent<Human>().curIncome);
                        GameSceneManager.Instance.sceneCanvas.ShowMoneyText(characterObj.transform.position+ Vector3.up, characterObj.GetComponent<Human>().curIncome);
                        animator.SetTrigger("Walk");
                        // ʹ�� DOTween ������ʼ�������ƶ�����
                        characterObj.transform.DOLookAt(destroyPoint[0], 0.1f).OnComplete(() =>
                        {
                            // �����ƶ�ʱ��
                            float duration = Vector3.Distance(characterObj.transform.position, destroyPoint[0]) / 2f;
                            // ʹ�� DOTween �������ƶ������ٵ�
                            characterObj.transform.DOMove(destroyPoint[0], duration).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                // ��������
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
        visitorLevel=PlayerPrefs.GetInt(G.VISITOR, 1);
        visitorLevel = Mathf.Clamp(visitorLevel, G.dc.gd.humanDatas[0].level, G.dc.gd.humanDataDataDict[G.dc.gd.humanDatas.Length - 1].level);
        if (G.dc.GetMoney() >= G.dc.gd.humanDataDataDict[visitorLevel].cost)
        {
            visitorLevel += 1;
            PlayerPrefs.SetInt(G.VISITOR, visitorLevel);
            GameManager.instance.UseHumanMoney(PlayerPrefs.GetInt(G.VISITOR, visitorLevel));
            Debug.Log(PlayerPrefs.GetInt(G.VISITOR, visitorLevel));
        }
        else
        {
            Debug.LogError("Ǯ�������޷���������");
        }


    }
    //G.dc.gd.fireWorkDataDict[level].income
}
