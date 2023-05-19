using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireWorkPhase
{
    Prepare, Fire, Nothing
}

[System.Serializable]
public class FireWorkModel
{
    public int fireWorkLevel;
    public int fireWorkIncome;
    public GameObject fireWorkObj;
    
}
public class FireWork : MonoBehaviour
{
    [SerializeField] List<FireWorkModel> fireWorkModels;
    public int curFireworkLevel=0;
    public int curFireworkIcome=0;
    public FireWorkPhase fwp;
    Animator animator;
    ParticleSystem particle;
    public void ShowModel(int level)
    {
        foreach (var item in fireWorkModels)
        {
            item.fireWorkObj.SetActive(item.fireWorkLevel == level);
            //particle=item.fireWorkObj.transform.GetChild(1).GetComponent<ParticleSystem>();
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayFx(GameObject obj, FireWorkPhase state)
    {
        foreach (var item in obj.GetComponentsInChildren<ParticleSystem>())
        {
            if (state == FireWorkPhase.Prepare)
            {
                particle = item;
                particle.Stop();
            }
            else
            if (state == FireWorkPhase.Fire)
            {
                particle = item;
                particle.Play();
            }
        }
       
       
        
    }
}
