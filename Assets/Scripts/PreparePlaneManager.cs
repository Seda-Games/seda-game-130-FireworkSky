using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparePlaneManager : MonoBehaviour
{
    public List<PreparePlane> preparePlanes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitPrepareFirePlane()
    {
        foreach (var item in preparePlanes)
        {
            int level=PlayerPrefs.GetInt("FireWorkLevel" + item.PreparePlaneID, 0);
            if (level > 0)
            {
                GameObject cub = Instantiate(GameManager.instance.fireWorkManager.firework, item.transform.position, Quaternion.identity);
                item.fireWork = cub.GetComponent<FireWork>();
                item.fireWork.ShowModel(level);
                cub.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[level].income;
                cub.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[level].level;
                cub.transform.parent = CameraManager.Instance.prepareRoot.transform;
            }
            //item.prepareLock.SetActive(false);
            if (PlayerPrefs.GetInt("PrepareUnlock" + item.PreparePlaneID, G.dc.gd.preparePlaneTableDict[item.PreparePlaneID].isunlock) == 1)
            {
                item.prepareLock.SetActive(false);
            }
            else
            {
                item.prepareLock.SetActive(true);
            }
            item.unlockcost.text = "$" + G.FormatNum(G.dc.gd.preparePlaneTableDict[item.PreparePlaneID].unlockcost);

        }
        for (int i = 0; i < preparePlanes.Count; i++)
        {

        }
    }
    public void InitNextMapPrepareFirePlane()
    {
        foreach (var item in preparePlanes)
        {
            int level = PlayerPrefs.GetInt("FireWorkLevel" + item.PreparePlaneID, 0);
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
                    cub.transform.parent = CameraManager.Instance.prepareRoot.transform;
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
                    cub.transform.parent = CameraManager.Instance.prepareRoot.transform;
                    PlayerPrefs.SetInt("FireWorkLevel" + item.PreparePlaneID, level);
                }
            }
            
        }
    }
    public void ResetFireWorkPosition()
    {
        foreach (var item in preparePlanes)
        {
            if (item.fireWork != null)
            {
                item.fireWork.transform.position = item.transform.position;
            }
        }
    }
    public void GetPreparePlaneID()
    {
        foreach (var item in preparePlanes)
        {
           int index= preparePlanes.IndexOf(item);
        }
    }
    public void UnlockPreparePlane()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
