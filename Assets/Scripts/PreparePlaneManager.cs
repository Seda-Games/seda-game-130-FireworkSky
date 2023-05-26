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
            }
        }
        for (int i = 0; i < preparePlanes.Count; i++)
        {

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
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
