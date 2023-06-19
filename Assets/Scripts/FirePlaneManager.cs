using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirePlaneManager : MonoBehaviour
{
    public List<FirePlane> firePlanes;
    // Start is called before the first frame update
    private int nextObjectToUnlock = 1004;
    int item1;
    public int unlockLevel;
    
    void Start()
    {
        
    }
    public void InitFirePlane()
    {
        
        foreach (var item in firePlanes)
        {
            int level = PlayerPrefs.GetInt("FireWorkLevel" + item.FirePlaneID, 0);
            if (level > 0)
            {
                GameObject cub = Instantiate(GameManager.instance.fireWorkManager.firework, item.transform.position, Quaternion.identity);
                item.fireWork = cub.GetComponent<FireWork>();
                item.fireWork.ShowModel(level);
                cub.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[level].income;
                cub.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[level].level;
                //item.fireWork.gameObject.transform.localScale = new Vector3(1f, 1f, 0.7f);
                //cub.transform.parent = CameraManager.Instance.prepareRoot.transform;
                item.fireWork.PlayFx(cub,FireWorkPhase.Fire);
            }
            /*
            if (item.FirePlaneID < 1004)
            {
                PlayerPrefs.SetInt("FirePlane" + item.FirePlaneID, 1);
            }
            bool isUnlocked = PlayerPrefs.GetInt("FirePlane" + item.FirePlaneID, 0) == 1;
            if (!isUnlocked)
            {
                item.Lock.SetActive(true);
                item.Unlock.SetActive(false);
            }
            else
            if(isUnlocked)
            {
                item.Lock.SetActive(false);
                item.Unlock.SetActive(true);
            }*/
            if (PlayerPrefs.GetInt("FireUnlock" + item.FirePlaneID, G.dc.gd.unlockFirePlaneTableDict[item.FirePlaneID].isunlock) == 1)
            {
                item.Lock.SetActive(false);
                item.Unlock.SetActive(true);
            }
            else
            {
                item.Lock.SetActive(true);
                item.Unlock.SetActive(false);
            }
            item.unlockcost.text = "$" + G.FormatNum(G.dc.gd.unlockFirePlaneTableDict[item.FirePlaneID].unlockcost);


            if (G.dc.gd.unlockFirePlaneTableDict[item.FirePlaneID].mapid <= PlayerPrefs.GetInt(G.MAP, 1))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }
    public void InitNextMapFirePlane()
    {
        foreach (var item in firePlanes)
        {
            int level = PlayerPrefs.GetInt("FireWorkLevel" + item.FirePlaneID, 0);
            if (item.fireWork != null)
            {
                Destroy(item.fireWork.gameObject);
            }
            if (level >= G.dc.gd.levelDict[PlayerPrefs.GetInt(G.MAP, 1)].firelevel)
            {
                if (level > 0)
                {
                    GameObject cub = Instantiate(GameManager.instance.fireWorkManager.firework, item.transform.position, Quaternion.identity);
                    item.fireWork = cub.GetComponent<FireWork>();
                    item.fireWork.ShowModel(level);
                    cub.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[level].income;
                    cub.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[level].level;
                    
                    item.fireWork.PlayFx(cub, FireWorkPhase.Fire);
                }
            }
            else
            {
                
                if (level > 0)
                {
                    level = G.dc.gd.levelDict[PlayerPrefs.GetInt(G.MAP, 1)].firelevel;
                    GameObject cub = Instantiate(GameManager.instance.fireWorkManager.firework, item.transform.position, Quaternion.identity);
                    item.fireWork = cub.GetComponent<FireWork>();
                    item.fireWork.ShowModel(level);
                    cub.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[level].income;
                    cub.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[level].level;
                    
                    item.fireWork.PlayFx(cub, FireWorkPhase.Fire);
                    PlayerPrefs.SetInt("FireWorkLevel" + item.FirePlaneID, level);
                }
            }
            if (G.dc.gd.unlockFirePlaneTableDict[item.FirePlaneID].mapid <= PlayerPrefs.GetInt(G.MAP, 1))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }

        }
    }
    public void UnlockFirePlane()
    {
        AudioManager.instance?.Tap();
        unlockLevel = PlayerPrefs.GetInt(G.UNLOCK, 2);
        unlockLevel = Mathf.Clamp(unlockLevel, G.dc.gd.firworkPlaneTables[0].level, G.dc.gd.firworkPlaneTables[G.dc.gd.firworkPlaneTables.Length - 1].level);
        if (G.dc.GetMoney() >= G.dc.gd.firworkPlaneTableDict[unlockLevel].unlockcost)
        {
            for (int i = 0; i < firePlanes.Count; i++)
            {
                int unlock = PlayerPrefs.GetInt("FirePlane" + firePlanes[i].FirePlaneID, 0);
                if (unlock == 0)
                {
                    item1 = i;
                    break;
                }
                else
                {
                    item1 = 7;
                }
            }
            if (item1 < 7 && unlockLevel < 6)
            {
                GameManager.instance.UnlockFirePlaneMoney(unlockLevel);
                unlockLevel += 1;
                PlayerPrefs.SetInt(G.UNLOCK, unlockLevel);
                if (unlockLevel == G.dc.gd.firworkPlaneTables.Length+1)
                {
                    GameManager.instance.bottomPanel.maxaddplatform.SetActive(true);
                    GameManager.instance.bottomPanel.IncomeButton.interactable = false;
                    GameManager.instance.bottomPanel.addFirePlaneText.SetActive(false);
                    GameManager.instance.ismax = true;
                }
                GameManager.instance.IsEnoughMoney();
                firePlanes[item1].Unlock.SetActive(true);
                firePlanes[item1].Lock.SetActive(false);
                PlayerPrefs.SetInt("FirePlane" + firePlanes[item1].FirePlaneID, 1);
                

            }
            else
            {

            }
        }
        else
        {
            Debug.LogError("钱不够，无法解锁");
        }
       

    }
    // Update is called once per frame
    void Update()
    {
        
    }
   
}
