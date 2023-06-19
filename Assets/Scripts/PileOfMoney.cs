using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOfMoney : ItemsBase
{
    private void Start()
    {
        //finishCallback += delegate { Destroy(gameObject); };
    }


    public override void OnClick()
    {
        int maxlevel = 0;
        foreach (var item in GameManager.instance.firePlaneManager.firePlanes)
        {
            if (item.fireWork != null)
            {
                if (item.fireWork.curFireworkLevel > maxlevel)
                {
                    maxlevel = item.fireWork.curFireworkLevel;
                }
            }
        }
        if (maxlevel > 0)
        {
            GameManager.instance.AddMoney(G.dc.gd.rewardMoneyTableDict[PlayerPrefs.GetInt(G.MAP, 1)].multiple * G.dc.gd.fireWorkDataDict[maxlevel].income);
            GameSceneManager.Instance.sceneCanvas.ShowMoneyText(GameManager.instance.fireWorkManager.element6.transform.position, G.dc.gd.rewardMoneyTableDict[PlayerPrefs.GetInt(G.MAP, 1)].multiple * G.dc.gd.fireWorkDataDict[maxlevel].income);
        }
        
        
        //GameManager.instance.AddMoney(characterObj.GetComponent<Human>().curIncome * G.dc.gd.achievementTableDict[num - 1].multiple);
        

        Destroy(gameObject);
        /*
        int starnum = PlayerPrefs.GetInt(G.SCENE_STAR + GameManager.Instance.WorldID, 0);
        float KmoneyA = G.dc.gd.Adsratiomsg.KMoney_A;
        float KmoneyB = G.dc.gd.Adsratiomsg.KMoney_B;
        float KmoneyC = G.dc.gd.Adsratiomsg.KMoney_C;
        float result = GameManager.Instance.SumOfAllUnits * KmoneyA + starnum * KmoneyB + KmoneyC;
        result = result * G.dc.gd.profitData.money;
        result = Mathf.Min(G.dc.gd.Adsratiomsg.KMoney_Max, result);
        GameManager.Instance.playUI.ads_MoneyUI.ShowPanel(new ResourcesUsed(ResourcesType.Money, (int)result), finishCallback);*/
    }

}
