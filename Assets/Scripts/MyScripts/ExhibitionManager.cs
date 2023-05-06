using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionManager : SingleInstance<ExhibitionManager>
{
    public Transform exhibition;
    public float rotateSpeed = 30;
    public GameObject exhibit;
    float angle = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if(exhibit!=null)
        {
            angle += Time.deltaTime * rotateSpeed;
            exhibition.eulerAngles = Vector3.up * angle;
        }
    }
    public void LoadModel(string path)
    {
        if(exhibit != null)
        {

            Destroy(exhibit.gameObject);
        }
        gameObject.SetActive(true);
        GameObject obj = Instantiate(Resources.Load<GameObject>(path), exhibition);
        obj.transform.position = Vector3.zero;
        this.exhibit = obj;
        List<Transform> transforms = new List<Transform>();
        Lin.GetAllChild(ref transforms,obj.transform);
        for(int i = 0; i < transforms.Count;++i)
        {
            if (transforms[i].gameObject.name.Equals("print"))
            {
                continue;
            }
            if(transforms[i].GetComponent<ParticleSystem>()!=null)
            {
                continue;
            }
            if (transforms[i].GetComponent<TrailRenderer>() != null)
            {
                continue;
            }
            transforms[i].gameObject.layer = Layer.EXHIBITION;
        }
    }
    public void LoadModelWithEffect(string path)
    {
        LoadModel(path);
    }
}
