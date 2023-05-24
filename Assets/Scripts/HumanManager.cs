using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HumanManager : MonoBehaviour
{
    public GameObject characterPrefab; // ����Ԥ����
    public Vector3 spawnPoint; // ������
    public Vector3 areaCenter; // �������ĵ�
    public Vector3 areaSize; // �����С
    public Vector3 destroyPoint;//������
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
        // ���Ƴ���������
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
    IEnumerator Visitor()
    {
        for (int i = 0; i < G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, 1)].flow; i++)
        {
            // �ڳ���������һ������
            GameObject characterObj = Instantiate(characterPrefab, spawnPoint, Quaternion.identity);
            Animator animator = characterObj.GetComponent<Animator>();
            characterObj.GetComponent<Human>().curLevel = G.dc.gd.humanDataDataDict[PlayerPrefs.GetInt(G.VISITOR, visitorLevel)].level;
            characterObj.GetComponent<Human>().curIncome= G.dc.gd.humanDataDataDict[characterObj.GetComponent<Human>().curLevel].income;
            // �������һ�������ڵĵ�
            Vector3 targetPoint = new Vector3(
                Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2),
                Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2),
                Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2)
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
                    // ��������Ŀ���ͣ��10��
                    DOVirtual.DelayedCall(10f, () =>
                    {
                        GameManager.instance.AddMoney(characterObj.GetComponent<Human>().curIncome);
                        GameSceneManager.Instance.sceneCanvas.ShowMoneyText(characterObj.transform.position+ Vector3.up, characterObj.GetComponent<Human>().curIncome);
                        animator.SetTrigger("Walk");
                        // ʹ�� DOTween ������ʼ�������ƶ�����
                        characterObj.transform.DOLookAt(destroyPoint, 0.1f).OnComplete(() =>
                        {
                            // �����ƶ�ʱ��
                            float duration = Vector3.Distance(characterObj.transform.position, destroyPoint) / 2f;
                            // ʹ�� DOTween �������ƶ������ٵ�
                            characterObj.transform.DOMove(destroyPoint, duration).SetEase(Ease.Linear).OnComplete(() =>
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
