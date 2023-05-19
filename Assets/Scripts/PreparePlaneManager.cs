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
            //int id=item.PreparePlaneID;
            /*if (item.fireWork.curFireworkLevel <= 0)
            {
                item.fireWork = null;
            }
            else
            if (item.fireWork.curFireworkLevel > 0)
            {
                int level = item.fireWork.curFireworkLevel;
                int income = item.fireWork.CurFireworkIcome;
                item.fireWork.transform.position = item.transform.position;
               
            }*/
            int level=PlayerPrefs.GetInt("FireWorkLevel" + item.PreparePlaneID, 0);
            if (level > 0)
            {
                GameObject cub = Instantiate(GameManager.instance.fireWorkManager.firework, item.transform.position, Quaternion.identity);
                item.fireWork = cub.GetComponent<FireWork>();
                item.fireWork.ShowModel(level);
                cub.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[level].income;
                cub.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[level].level;
            }
           
            //PlayerPrefs.SetInt("FireWorkLevel" + item.PreparePlaneID, level);
            Debug.Log("多少"+ level);
        }
        for (int i = 0; i < preparePlanes.Count; i++)
        {
            //Debug.Log("输出对应的id"+preparePlanes[i].PreparePlaneID);
        }
    }

    public void GetPreparePlaneID()
    {
        foreach (var item in preparePlanes)
        {
           int index= preparePlanes.IndexOf(item);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
