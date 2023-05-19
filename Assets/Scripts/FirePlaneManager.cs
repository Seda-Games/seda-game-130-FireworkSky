using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlaneManager : MonoBehaviour
{
    public List<FirePlane> firePlanes;
    // Start is called before the first frame update

    
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
                item.fireWork.PlayFx(cub,FireWorkPhase.Fire);
            }
            /*int level = PlayerPrefs.GetInt("FireWorkLevel" + item.FirePlaneID, 0);
            GameObject cub = Instantiate(GameManager.instance.fireWorkManager.firework, item.transform.position, Quaternion.identity);
            item.fireWork = cub.GetComponent<FireWork>();
            GameManager.instance.fireWork.ShowModel(level);
            PlayerPrefs.SetInt("FireWorkLevel" + item.FirePlaneID, level);
            Debug.Log("∂‡…Ÿ" + level);

            if (item.fireWork.curFireworkLevel <= 0)
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
            //int level = PlayerPrefs.GetInt("FireWorkLevel" + item.FirePlaneID,GameManager.instance.fireWorkManager.fireWork.curFireworkLevel);
        }
        for (int i = 0; i < firePlanes.Count; i++)
        {
            //int id =firePlanes[i].FirePlaneID;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
