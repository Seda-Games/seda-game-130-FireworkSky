using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorkManager : MonoBehaviour
{

    public GameObject firework;
    private Vector3 originPos;
    public Vector3 originalPosition;
    private Vector3 currentPos;
    private Vector3 lastPos;
    private Vector3 deltaPos;
    private Vector3 tempHitPoint;//物体移动前的位置
    public string layerMask;//移动检测层
    public float Depth_Z;//不在平面上移动的摄像机看到物体位置深度
    bool is_element = false;
    public GameObject element;
    public GameObject element2;
    public GameObject element3;
    public GameObject element4;
    public GameObject element5;
    public GameObject element6;
    public FireWork fireWork;
    public List<GameObject> fireworkNum;
    int item1;
    public GameObject cub;
    int currentSpawnIndex=0;
    public List<GameObject> gameObjects;
    public GameObject newfirework;
    public ParticleSystem particleSystem;
    bool isGet=true;
    public int fireWorkLevel;
    public int addIncomelevel;
    public GameObject particlesystem;
    public GameObject[] slide;
    public int launcher = 0;
    public int stage;
    public bool isachieve;
    public float duration=0;
    public bool isfinish = false;
    public int maxlevel;
    public int times;
    void Awake()
    {
        //InitFireWork();
        

    }
    // Start is called before the first frame update

    void Start()
    {
        launcher = PlayerPrefs.GetInt(G.ACHIEVEMENT, 0);
        stage = PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1);
    }
    // Update is called once per frame
    void Update()
    {
        ResetLastPos();
        ItemMove();
        GetFireWorkIncome();
       /* if (isachieve == true)
        {
            duration += Time.deltaTime;
            //Debug.Log("现在是多少秒" + duration);
            int num = Mathf.Clamp(PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, stage), G.dc.gd.achievementTables[0].level, G.dc.gd.achievementTableDict[G.dc.gd.achievementTables.Length].level);
            if (duration > G.dc.gd.achievementTableDict[num].duration)
            {
                isachieve = false;
                duration = 0;

            }
        }*/
        

    }


    public void ItemMove()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag(Tag.PreparePlane))
                {
                    element2 = hit.collider.gameObject;
                    Debug.Log(hit.transform.tag);

                }
                else
                if (hit.transform.CompareTag(Tag.FirePlane))
                {
                    element2 = hit.collider.gameObject;
                    Debug.Log(hit.transform.tag);

                }
                else
                if (hit.transform.CompareTag(Tag.PrepareUnlock))
                {
                    element4 = hit.collider.gameObject;
                    Debug.Log(hit.transform.tag);
                }
                else
                if (hit.transform.CompareTag(Tag.FireUnlock))
                {
                    element4 = hit.collider.gameObject;
                    Debug.Log(hit.transform.tag);
                }
                else
                if (hit.transform.CompareTag(Tag.SceneMoney))
                {
                    element5 = hit.collider.gameObject;
                    Debug.Log(hit.transform.tag);
                }
                if (hit.transform.CompareTag(Tag.FireWork))
                {
                    element = hit.collider.gameObject;
                }
            }
            /*if (Physics.Raycast(ray, out hit, int.MaxValue, 1 << Layer.Plane << Layer.FireWork))
            {
                Debug.Log("dasdasda");
                if (hit.transform.CompareTag(Tag.PreparePlane))
                {
                    element2 = hit.collider.gameObject;
                    Debug.Log(hit.transform.tag);
                    
                }
                else
                if (hit.transform.CompareTag(Tag.FirePlane))
                {
                    element2 = hit.collider.gameObject;
                    Debug.Log(hit.transform.tag);
                }
            }
            
            if (Physics.Raycast(ray, out hit, int.MaxValue, 1 << Layer.FireWork))
            {
                if (hit.transform.CompareTag(Tag.FireWork))
                {
                    element = hit.collider.gameObject;
                    Debug.Log("射线检测到了烟花");
                }
            }*/
            isGet = true;
        }

        if (Input.GetMouseButton(0))//开始移动
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].GetComponent<BoxCollider>().enabled = false;
            }
            if (element != null)
            {
                if (element2 != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << Layer.Fire))
                    {
                        Debug.Log("ddddddddddddddddddddddddddddddddddddddd");
                        if (hit.transform.CompareTag(Tag.FireWork))
                        {
                            element = hit.collider.gameObject;

                            if (isGet == true)
                            {
                                foreach (var item in FindObjectsOfType<FireWork>())
                                {
                                    fireworkNum.Add(item.gameObject);
                                }
                                foreach (var item in fireworkNum)
                                {
                                    if (item != element)
                                    {
                                        if (item.GetComponent<BoxCollider>() != null)
                                        {
                                            item.GetComponent<BoxCollider>().enabled = false;
                                        }
                                    }

                                }
                                isGet = false;
                            }


                            element.layer = LayerMask.NameToLayer("Ignore Raycast");//物体不检测射线，避免挡住鼠标检测移动层
                            StartCoroutine(ItemMove(element));
                            Debug.Log("移动了物体");

                        }
                        Debug.Log("1");
                        //Debug.Log(hit.transform.gameObject);
                    }
                }
            }
           
            

        }
        if (Input.GetMouseButtonUp(0))//停止移动
        {
            foreach (var item in fireworkNum)
            {
               if (item.GetComponent<BoxCollider>() != null)
               {
                   item.GetComponent<BoxCollider>().enabled = true;
               }
            }
            fireworkNum.Clear();
            /*if (element)
            {
                element.layer = LayerMask.NameToLayer("Fire");//物体回到初始层
                Debug.Log(element.layer);
            }*/


            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].GetComponent<BoxCollider>().enabled = true;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, int.MaxValue, 1 << Layer.REWARDMONEY))
            {
                if (hit.transform.CompareTag(Tag.SceneMoney))
                {
                    element6 = hit.collider.gameObject;
                    if (element5 == element6)
                    {
                        hit.transform.GetComponent<ItemsBase>().OnClick();
                        element5 = null;
                        element6 = null;
                    }
                }
            }
            else
            if (Physics.Raycast(ray, out hit, int.MaxValue, 1 << Layer.UNLOCK))
            {
                //解锁准备台
                if (hit.transform.CompareTag(Tag.PrepareUnlock))
                {
                    element3 = hit.collider.gameObject;
                    if (element)
                    {
                        element.transform.position = element2.transform.position;
                        element = null;
                        element2 = null;
                        element3 = null;
                        element4 = null;
                    }
                    else
                    if (G.dc.GetMoney() >= G.dc.gd.preparePlaneTableDict[element3.transform.parent.GetComponent<PreparePlane>().PreparePlaneID].unlockcost && element4 == element3)
                    {
                        element3.SetActive(false);
                        element3.transform.parent.GetComponent<PreparePlane>().isUnlock = true;
                        PlayerPrefs.SetInt("PrepareUnlock" + element3.transform.parent.GetComponent<PreparePlane>().PreparePlaneID, 1);
                        GameManager.instance.UnlockPreparePlaneMoney(element3.transform.parent.GetComponent<PreparePlane>().PreparePlaneID);
                    }
                    else
                    if (element4 == element3)
                    {
                        ToastManager.Show("Not enough money to unlock");
                        //Debug.LogError("钱不够，无法继续解锁");
                    }
                    element4 = null;
                    element3 = null;
                }
                else
                //解锁发射台
                if (hit.transform.CompareTag(Tag.FireUnlock))
                {
                    element3 = hit.collider.gameObject;
                    if (element)
                    {
                        element.transform.position = element2.transform.position;
                        element = null;
                        element2 = null;
                        element3 = null;
                        element4 = null;
                    }
                    else
                    if (G.dc.GetMoney() >= G.dc.gd.unlockFirePlaneTableDict[element3.transform.parent.GetComponent<FirePlane>().FirePlaneID].unlockcost && element4 == element3)
                    {
                        element3.SetActive(false);
                        element3.transform.parent.GetComponent<FirePlane>().isUnlock = true;
                        element3.transform.parent.GetComponent<FirePlane>().Unlock.SetActive(true);
                        PlayerPrefs.SetInt("FireUnlock" + element3.transform.parent.GetComponent<FirePlane>().FirePlaneID, 1);
                        GameManager.instance.UnlockFireMoney(element3.transform.parent.GetComponent<FirePlane>().FirePlaneID);
                    }
                    else
                    if (element4 == element3)
                    {
                        ToastManager.Show("Not enough money to unlock");
                        //Debug.LogError("钱不够，无法继续解锁");
                    }
                    element4 = null;
                    element3 = null;
                }
            }
            else
            if (Physics.Raycast(ray, out hit, int.MaxValue, 1 << Layer.Plane))
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10000, Color.red);
                Debug.Log("????????" + hit.transform.name);
                //element.layer = LayerMask.NameToLayer("Ignore Raycast");//物体不检测射线，避免挡住鼠标检测移动层
                //Debug.Log(element.layer);
                if (element)
                {
                    if (hit.transform.CompareTag(Tag.FirePlane))
                    {
                        element3 = hit.collider.gameObject;
                        if (element2.transform.position == element3.transform.position)
                        {
                            if (element)
                            {
                                element.GetComponent<FireWork>().fwp = FireWorkPhase.Fire;
                                element.GetComponent<FireWork>().PlayFx(element, FireWorkPhase.Fire);
                                element.transform.position = element2.transform.position;
                                element.transform.parent = GameManager.Instance.fireroot.transform;
                            }

                            element = null;
                            element2 = null;
                            element3 = null;
                        }
                        else
                        if (element2.transform.position != element3.transform.position)
                        {
                            if (element)
                            {
                                element.GetComponent<FireWork>().fwp = FireWorkPhase.Fire;
                                element.GetComponent<FireWork>().PlayFx(element, FireWorkPhase.Fire);
                                element.transform.position = element3.transform.position;
                                element.transform.parent = GameManager.Instance.fireroot.transform;
                            }

                            if (element2.tag == Tag.FirePlane && element2.GetComponent<FirePlane>().fireWork != null)
                            {
                                if (element3)
                                {
                                    if (element3.GetComponent<FirePlane>().fireWork == null)
                                    {
                                        element3.GetComponent<FirePlane>().fireWork = element2.GetComponent<FirePlane>().fireWork;
                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, 0);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, element3.GetComponent<FirePlane>().fireWork.curFireworkLevel);
                                        element2.GetComponent<FirePlane>().fireWork = null;
                                    }
                                    else
                                    if (element3.GetComponent<FirePlane>().fireWork != null && element2.GetComponent<FirePlane>().fireWork != null)
                                    {
                                        if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel == element2.GetComponent<FirePlane>().fireWork.curFireworkLevel && element3.GetComponent<FirePlane>().fireWork.curFireworkLevel != G.dc.gd.fireWorkDataDict[G.dc.gd.fireWorkDatas.Length].level)
                                        {
                                            //生成新烟花
                                            newfirework = Instantiate(firework, element3.transform.position, Quaternion.identity);
                                            newfirework.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel + 1].level;
                                            newfirework.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel + 1].income;
                                            GameSceneManager.Instance.sceneCanvas.ShowUpgradeFx(element3.transform.position);
                                            //Instantiate(particlesystem, element3.transform.position, Quaternion.identity);
                                            //销毁原来的对象
                                            Destroy(element3.GetComponent<FirePlane>().fireWork.gameObject);
                                            Destroy(element2.GetComponent<FirePlane>().fireWork.gameObject);

                                            //保存新烟花
                                            element3.GetComponent<FirePlane>().fireWork = newfirework.GetComponent<FireWork>();
                                            newfirework.GetComponent<FireWork>().ShowModel(newfirework.GetComponent<FireWork>().curFireworkLevel);
                                            newfirework.GetComponent<FireWork>().fwp = FireWorkPhase.Fire;
                                            newfirework.GetComponent<FireWork>().PlayFx(newfirework, FireWorkPhase.Fire);

                                            newfirework.transform.parent = GameManager.Instance.fireroot.transform;
                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, 0);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);


                                            GameManager.instance.NewFirework.ShowUI(G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                            PlayerPrefs.SetInt("Rocket" + G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level, 1);
                                            foreach (var item in GameManager.instance.firePlaneManager.firePlanes)
                                            {
                                                if (item.fireWork != null)
                                                {
                                                    Debug.Log("到底是多少级");
                                                    if (item.fireWork.curFireworkLevel > maxlevel)
                                                    {
                                                        maxlevel = item.fireWork.curFireworkLevel;
                                                    }
                                                    

                                                }
                                            }
                                            if (maxlevel >= 17 && maxlevel < 21)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 5);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();
                                                
                                            }
                                            else
                                            if (maxlevel >= 13 && maxlevel < 17)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 4);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 9 && maxlevel < 13)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 3);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 5 && maxlevel < 9)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 2);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();
                                            }
                                            element2.GetComponent<FirePlane>().fireWork = null;
                                        }
                                        else
                                        if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel != element2.GetComponent<FirePlane>().fireWork.curFireworkLevel)
                                        {
                                            //交换位置 
                                            element2.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                            element3.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element2.transform.position;

                                            FireWork temp;
                                            temp = element2.GetComponent<FirePlane>().fireWork;
                                            element2.GetComponent<FirePlane>().fireWork = element3.GetComponent<FirePlane>().fireWork;
                                            element3.GetComponent<FirePlane>().fireWork = temp;






                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);
                                        }
                                        else if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel == element2.GetComponent<FirePlane>().fireWork.curFireworkLevel && element3.GetComponent<FirePlane>().fireWork.curFireworkLevel == G.dc.gd.fireWorkDataDict[G.dc.gd.fireWorkDatas.Length].level)
                                        {
                                            //交换位置 
                                            element2.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                            element3.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element2.transform.position;

                                            FireWork temp;
                                            temp = element2.GetComponent<FirePlane>().fireWork;
                                            element2.GetComponent<FirePlane>().fireWork = element3.GetComponent<FirePlane>().fireWork;
                                            element3.GetComponent<FirePlane>().fireWork = temp;






                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);
                                        }
                                    }


                                }

                            }
                            else
                            if (element2.tag == Tag.PreparePlane && element2.GetComponent<PreparePlane>().fireWork != null)
                            {
                                if (element3)
                                {
                                    if (element3.GetComponent<FirePlane>().fireWork == null)
                                    {
                                        element3.GetComponent<FirePlane>().fireWork = element2.GetComponent<PreparePlane>().fireWork;
                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, 0);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, element3.GetComponent<FirePlane>().fireWork.curFireworkLevel);
                                        element3.GetComponent<FirePlane>().fireWork.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                                        foreach (var item in GameManager.instance.firePlaneManager.firePlanes)
                                        {
                                            if (item.fireWork != null)
                                            {
                                                Debug.Log("到底是多少级");
                                                if (item.fireWork.curFireworkLevel > maxlevel)
                                                {
                                                    maxlevel = item.fireWork.curFireworkLevel;
                                                }


                                            }
                                        }
                                        if (maxlevel >= 17 && maxlevel < 21)
                                        {
                                            PlayerPrefs.SetInt(G.STAGE, 5);
                                            CameraManager.Instance.MoveToTarget();
                                            ShowOrHideSlide();

                                        }
                                        else
                                        if (maxlevel >= 13 && maxlevel < 17)
                                        {
                                            PlayerPrefs.SetInt(G.STAGE, 4);
                                            CameraManager.Instance.MoveToTarget();
                                            ShowOrHideSlide();

                                        }
                                        else
                                        if (maxlevel >= 9 && maxlevel < 13)
                                        {
                                            PlayerPrefs.SetInt(G.STAGE, 3);
                                            CameraManager.Instance.MoveToTarget();
                                            ShowOrHideSlide();

                                        }
                                        else
                                        if (maxlevel >= 5 && maxlevel < 9)
                                        {
                                            PlayerPrefs.SetInt(G.STAGE, 2);
                                            CameraManager.Instance.MoveToTarget();
                                            ShowOrHideSlide();
                                        }
                                        element2.GetComponent<PreparePlane>().fireWork = null;
                                    }
                                    else
                                    if (element3.GetComponent<FirePlane>().fireWork != null && element2.GetComponent<PreparePlane>().fireWork != null)
                                    {
                                        if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel == element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel && element3.GetComponent<FirePlane>().fireWork.curFireworkLevel != G.dc.gd.fireWorkDataDict[G.dc.gd.fireWorkDatas.Length].level)
                                        {
                                            //生成新烟花
                                            newfirework = Instantiate(firework, element3.transform.position, Quaternion.identity);
                                            newfirework.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel + 1].level;
                                            newfirework.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel + 1].income;
                                            GameSceneManager.Instance.sceneCanvas.ShowUpgradeFx(element3.transform.position);
                                            //Instantiate(particlesystem, element3.transform.position, Quaternion.identity);
                                            //销毁原来的对象
                                            Destroy(element3.GetComponent<FirePlane>().fireWork.gameObject);
                                            Destroy(element2.GetComponent<PreparePlane>().fireWork.gameObject);

                                            //保存新烟花
                                            element3.GetComponent<FirePlane>().fireWork = newfirework.GetComponent<FireWork>();
                                            newfirework.GetComponent<FireWork>().ShowModel(newfirework.GetComponent<FireWork>().curFireworkLevel);
                                            newfirework.GetComponent<FireWork>().fwp = FireWorkPhase.Fire;
                                            newfirework.GetComponent<FireWork>().PlayFx(newfirework, FireWorkPhase.Fire);
                                            newfirework.transform.parent = GameManager.Instance.fireroot.transform;
                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, 0);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                            //element3.GetComponent<FirePlane>().fireWork.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                                            GameManager.instance.NewFirework.ShowUI(G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                            PlayerPrefs.SetInt("Rocket" + G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level, 1);
                                            foreach (var item in GameManager.instance.firePlaneManager.firePlanes)
                                            {
                                                if (item.fireWork != null)
                                                {
                                                    Debug.Log("到底是多少级");
                                                    if (item.fireWork.curFireworkLevel > maxlevel)
                                                    {
                                                        maxlevel = item.fireWork.curFireworkLevel;
                                                    }


                                                }
                                            }
                                            if (maxlevel >= 17 && maxlevel < 21)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 5);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 13 && maxlevel < 17)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 4);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 9 && maxlevel < 13)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 3);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 5 && maxlevel < 9)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 2);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();
                                            }
                                            element2.GetComponent<PreparePlane>().fireWork = null;
                                        }
                                        else
                                        if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel != element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel)
                                        {
                                            //交换位置

                                            element2.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                            element3.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element2.transform.position;

                                            element2.GetComponent<PreparePlane>().fireWork.gameObject.transform.parent = GameManager.instance.fireroot.transform;
                                            element3.GetComponent<FirePlane>().fireWork.gameObject.transform.parent = CameraManager.Instance.prepareRoot.transform;

                                            FireWork temp;
                                            temp = element2.GetComponent<PreparePlane>().fireWork;
                                            element2.GetComponent<PreparePlane>().fireWork = element3.GetComponent<FirePlane>().fireWork;
                                            element3.GetComponent<FirePlane>().fireWork = temp;


                                            element2.GetComponent<PreparePlane>().fireWork.fwp = FireWorkPhase.Prepare;
                                            element2.GetComponent<PreparePlane>().fireWork.PlayFx(element2.GetComponent<PreparePlane>().fireWork.gameObject, FireWorkPhase.Prepare);
                                            element2.GetComponent<PreparePlane>().fireWork.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                                            element3.GetComponent<FirePlane>().fireWork.fwp = FireWorkPhase.Fire;
                                            element3.GetComponent<FirePlane>().fireWork.PlayFx(element3.GetComponent<FirePlane>().fireWork.gameObject, FireWorkPhase.Fire);
                                            element3.GetComponent<FirePlane>().fireWork.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);


                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);

                                            foreach (var item in GameManager.instance.firePlaneManager.firePlanes)
                                            {
                                                if (item.fireWork != null)
                                                {
                                                    Debug.Log("到底是多少级");
                                                    if (item.fireWork.curFireworkLevel > maxlevel)
                                                    {
                                                        maxlevel = item.fireWork.curFireworkLevel;
                                                    }


                                                }
                                            }
                                            if (maxlevel >= 17 && maxlevel < 21)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 5);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 13 && maxlevel < 17)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 4);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 9 && maxlevel < 13)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 3);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 5 && maxlevel < 9)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 2);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();
                                            }
                                        }
                                        else
                                        if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel == element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel && element3.GetComponent<FirePlane>().fireWork.curFireworkLevel == G.dc.gd.fireWorkDataDict[G.dc.gd.fireWorkDatas.Length].level)
                                        {
                                            //交换位置

                                            element2.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                            element3.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element2.transform.position;

                                            element2.GetComponent<PreparePlane>().fireWork.gameObject.transform.parent = GameManager.instance.fireroot.transform;
                                            element3.GetComponent<FirePlane>().fireWork.gameObject.transform.parent = CameraManager.Instance.prepareRoot.transform;

                                            FireWork temp;
                                            temp = element2.GetComponent<PreparePlane>().fireWork;
                                            element2.GetComponent<PreparePlane>().fireWork = element3.GetComponent<FirePlane>().fireWork;
                                            element3.GetComponent<FirePlane>().fireWork = temp;


                                            element2.GetComponent<PreparePlane>().fireWork.fwp = FireWorkPhase.Prepare;
                                            element2.GetComponent<PreparePlane>().fireWork.PlayFx(element2.GetComponent<PreparePlane>().fireWork.gameObject, FireWorkPhase.Prepare);
                                            element2.GetComponent<PreparePlane>().fireWork.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                                            element3.GetComponent<FirePlane>().fireWork.fwp = FireWorkPhase.Fire;
                                            element3.GetComponent<FirePlane>().fireWork.PlayFx(element3.GetComponent<FirePlane>().fireWork.gameObject, FireWorkPhase.Fire);
                                            element3.GetComponent<FirePlane>().fireWork.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);


                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);
                                        }
                                    }

                                }

                            }
                            //element.transform.parent = CameraManager.Instance.prepareRoot.transform;
                            element = null;
                            element2 = null;
                            element3 = null;
                        }
                        Debug.Log("检测到在发射台");
                    }
                    else
                    if (hit.transform.CompareTag(Tag.PreparePlane))
                    {
                        element3 = hit.collider.gameObject;

                        Debug.Log("这个物体是什么" + element3);
                        if (element2.transform.position == element3.transform.position)
                        {
                            if (element)
                            {
                                element.GetComponent<FireWork>().fwp = FireWorkPhase.Prepare;
                                element.GetComponent<FireWork>().PlayFx(element, FireWorkPhase.Prepare);
                                element.transform.position = element2.transform.position;
                                element.transform.parent = CameraManager.Instance.prepareRoot.transform;
                            }

                            element = null;
                            element2 = null;
                            element3 = null;
                        }
                        else
                        if (element2.transform.position != element3.transform.position)
                        {
                            if (element)
                            {
                                element.GetComponent<FireWork>().fwp = FireWorkPhase.Prepare;
                                element.GetComponent<FireWork>().PlayFx(element, FireWorkPhase.Prepare);
                                element.transform.position = element3.transform.position;
                                element.transform.parent = CameraManager.Instance.prepareRoot.transform;
                            }
                            if (element2.tag == Tag.FirePlane && element2.GetComponent<FirePlane>().fireWork != null)
                            {//elemen3是PreparePlane,element2是FirePlane
                                if (element3)
                                {
                                    if (element3.GetComponent<PreparePlane>().fireWork == null)
                                    {
                                        element3.GetComponent<PreparePlane>().fireWork = element2.GetComponent<FirePlane>().fireWork;
                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, 0);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel);
                                        element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                                        element2.GetComponent<FirePlane>().fireWork = null;
                                        element.transform.parent = CameraManager.Instance.prepareRoot.transform;
                                    }
                                    if (element3.GetComponent<PreparePlane>().fireWork != null && element2.GetComponent<FirePlane>().fireWork != null)
                                    {
                                        if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel == element2.GetComponent<FirePlane>().fireWork.curFireworkLevel && element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel != G.dc.gd.fireWorkDataDict[G.dc.gd.fireWorkDatas.Length].level)
                                        {
                                            //生成新烟花
                                            newfirework = Instantiate(firework, element3.transform.position, Quaternion.identity);
                                            newfirework.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel + 1].level;
                                            newfirework.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel + 1].income;
                                            GameSceneManager.Instance.sceneCanvas.ShowUpgradeFx(element3.transform.position);
                                            //Instantiate(particlesystem, element3.transform.position, Quaternion.identity);
                                            //销毁原来的对象
                                            Destroy(element3.GetComponent<PreparePlane>().fireWork.gameObject);
                                            Destroy(element2.GetComponent<FirePlane>().fireWork.gameObject);

                                            //保存新烟花
                                            element3.GetComponent<PreparePlane>().fireWork = newfirework.GetComponent<FireWork>();
                                            newfirework.GetComponent<FireWork>().ShowModel(newfirework.GetComponent<FireWork>().curFireworkLevel);
                                            newfirework.transform.parent = CameraManager.Instance.prepareRoot.transform;
                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, 0);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                                            GameManager.instance.NewFirework.ShowUI(G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                            PlayerPrefs.SetInt("Rocket" + G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level, 1);
                                            foreach (var item in GameManager.instance.firePlaneManager.firePlanes)
                                            {
                                                if (item.fireWork != null)
                                                {
                                                    Debug.Log("到底是多少级");
                                                    if (item.fireWork.curFireworkLevel > maxlevel)
                                                    {
                                                        maxlevel = item.fireWork.curFireworkLevel;
                                                    }


                                                }
                                            }
                                            if (maxlevel >= 17 && maxlevel < 21)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 5);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 13 && maxlevel < 17)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 4);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 9 && maxlevel < 13)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 3);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 5 && maxlevel < 9)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 2);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();
                                            }
                                            element2.GetComponent<FirePlane>().fireWork = null;
                                        }
                                        else
                                        if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel != element2.GetComponent<FirePlane>().fireWork.curFireworkLevel)
                                        {
                                            //交换位置
                                            element2.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element2.transform.position;

                                            element2.GetComponent<FirePlane>().fireWork.gameObject.transform.parent = CameraManager.Instance.prepareRoot.transform;
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.parent = GameManager.instance.fireroot.transform;

                                            FireWork temp;
                                            temp = element2.GetComponent<FirePlane>().fireWork;
                                            element2.GetComponent<FirePlane>().fireWork = element3.GetComponent<PreparePlane>().fireWork;
                                            element3.GetComponent<PreparePlane>().fireWork = temp;

                                            element2.GetComponent<FirePlane>().fireWork.fwp = FireWorkPhase.Fire;
                                            element2.GetComponent<FirePlane>().fireWork.PlayFx(element2.GetComponent<FirePlane>().fireWork.gameObject, FireWorkPhase.Fire);
                                            element2.GetComponent<FirePlane>().fireWork.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

                                            element3.GetComponent<PreparePlane>().fireWork.fwp = FireWorkPhase.Prepare;
                                            element3.GetComponent<PreparePlane>().fireWork.PlayFx(element3.GetComponent<PreparePlane>().fireWork.gameObject, FireWorkPhase.Prepare);
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);



                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);

                                            foreach (var item in GameManager.instance.firePlaneManager.firePlanes)
                                            {
                                                if (item.fireWork != null)
                                                {
                                                    Debug.Log("到底是多少级");
                                                    if (item.fireWork.curFireworkLevel > maxlevel)
                                                    {
                                                        maxlevel = item.fireWork.curFireworkLevel;
                                                    }


                                                }
                                            }
                                            if (maxlevel >= 17 && maxlevel < 21)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 5);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 13 && maxlevel < 17)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 4);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 9 && maxlevel < 13)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 3);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();

                                            }
                                            else
                                            if (maxlevel >= 5 && maxlevel < 9)
                                            {
                                                PlayerPrefs.SetInt(G.STAGE, 2);
                                                CameraManager.Instance.MoveToTarget();
                                                ShowOrHideSlide();
                                            }
                                        }
                                        else if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel == element2.GetComponent<FirePlane>().fireWork.curFireworkLevel && element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel == G.dc.gd.fireWorkDataDict[G.dc.gd.fireWorkDatas.Length].level)
                                        {
                                            //交换位置
                                            element2.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element2.transform.position;

                                            element2.GetComponent<FirePlane>().fireWork.gameObject.transform.parent = CameraManager.Instance.prepareRoot.transform;
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.parent = GameManager.instance.fireroot.transform;

                                            FireWork temp;
                                            temp = element2.GetComponent<FirePlane>().fireWork;
                                            element2.GetComponent<FirePlane>().fireWork = element3.GetComponent<PreparePlane>().fireWork;
                                            element3.GetComponent<PreparePlane>().fireWork = temp;

                                            element2.GetComponent<FirePlane>().fireWork.fwp = FireWorkPhase.Fire;
                                            element2.GetComponent<FirePlane>().fireWork.PlayFx(element2.GetComponent<FirePlane>().fireWork.gameObject, FireWorkPhase.Fire);
                                            element2.GetComponent<FirePlane>().fireWork.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

                                            element3.GetComponent<PreparePlane>().fireWork.fwp = FireWorkPhase.Prepare;
                                            element3.GetComponent<PreparePlane>().fireWork.PlayFx(element3.GetComponent<PreparePlane>().fireWork.gameObject, FireWorkPhase.Prepare);
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);



                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);
                                        }
                                    }


                                }

                            }
                            else
                            if (element2.tag == Tag.PreparePlane && element2.GetComponent<PreparePlane>().fireWork != null)
                            {//elemen3是PreparePlane,element2是PreparePlane
                                if (element3)
                                {
                                    if (element3.GetComponent<PreparePlane>().fireWork == null)
                                    {
                                        element3.GetComponent<PreparePlane>().fireWork = element2.GetComponent<PreparePlane>().fireWork;
                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, 0);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel);
                                        element2.GetComponent<PreparePlane>().fireWork = null;
                                        element.transform.parent = CameraManager.Instance.prepareRoot.transform;
                                    }
                                    if (element3.GetComponent<PreparePlane>().fireWork != null && element2.GetComponent<PreparePlane>().fireWork != null)
                                    {
                                        if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel == element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel && element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel != G.dc.gd.fireWorkDataDict[G.dc.gd.fireWorkDatas.Length].level)
                                        {
                                            //生成新烟花
                                            newfirework = Instantiate(firework, element3.transform.position, Quaternion.identity);
                                            newfirework.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel + 1].level;
                                            newfirework.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel + 1].income;
                                            GameSceneManager.Instance.sceneCanvas.ShowUpgradeFx(element3.transform.position);
                                            //Instantiate(particlesystem, element3.transform.position, Quaternion.identity);
                                            //销毁原来的对象
                                            Destroy(element3.GetComponent<PreparePlane>().fireWork.gameObject);
                                            Destroy(element2.GetComponent<PreparePlane>().fireWork.gameObject);

                                            //保存新烟花
                                            element3.GetComponent<PreparePlane>().fireWork = newfirework.GetComponent<FireWork>();
                                            newfirework.GetComponent<FireWork>().ShowModel(newfirework.GetComponent<FireWork>().curFireworkLevel);
                                            newfirework.transform.parent = CameraManager.Instance.prepareRoot.transform;
                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, 0);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                                            GameManager.instance.NewFirework.ShowUI(G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                            PlayerPrefs.SetInt("Rocket" + G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level, 1);
                                            element2.GetComponent<PreparePlane>().fireWork = null;


                                        }
                                        else
                                        if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel != element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel)
                                        {//交换位置
                                            element2.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element2.transform.position;

                                            FireWork temp;
                                            temp = element2.GetComponent<PreparePlane>().fireWork;
                                            element2.GetComponent<PreparePlane>().fireWork = element3.GetComponent<PreparePlane>().fireWork;
                                            element3.GetComponent<PreparePlane>().fireWork = temp;






                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);
                                        }
                                        else
                                        if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel == element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel && element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel == G.dc.gd.fireWorkDataDict[G.dc.gd.fireWorkDatas.Length].level)
                                        {
                                            //交换位置
                                            element2.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                            element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element2.transform.position;

                                            FireWork temp;
                                            temp = element2.GetComponent<PreparePlane>().fireWork;
                                            element2.GetComponent<PreparePlane>().fireWork = element3.GetComponent<PreparePlane>().fireWork;
                                            element3.GetComponent<PreparePlane>().fireWork = temp;






                                            PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);
                                            PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);
                                        }
                                    }

                                }

                            }
                            element.transform.parent = CameraManager.Instance.prepareRoot.transform;
                            element = null;
                            element2 = null;
                            element3 = null;
                        }
                        Debug.Log("检测到在准备台");
                    }
                    else
                    if (!hit.transform.CompareTag(Tag.FirePlane) && !hit.transform.CompareTag(Tag.PreparePlane) && !hit.transform.CompareTag(Tag.PrepareUnlock))
                    {
                        if (element)
                        {

                            element.transform.position = element2.transform.position;
                            element = null;
                            element2 = null;
                            element3 = null;
                        }
                    }
                }

            }
            else
            {
                if (element)
                {

                    element.transform.position = element2.transform.position;
                    element = null;
                    element2 = null;
                }

            }
            //element.transform.parent = CameraManager.Instance.prepareRoot.transform;
            element = null;
            element2 = null;
            element3 = null;
        }
        if (element)
        {
            element.layer = LayerMask.NameToLayer("Fire");//物体回到初始层
        }

    }

    public IEnumerator ItemMove(GameObject item)
    {
        Vector3 SpaceScreen = Camera.main.WorldToScreenPoint(Camera.main.transform.position);//屏幕上移动的深度
        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (item)
            {
                currentPos = Input.mousePosition;
                if (lastPos != Vector3.zero)
                {
                    deltaPos = currentPos - lastPos;
                }

                lastPos = currentPos;
                Vector3 itemScreenPos = Camera.main.WorldToScreenPoint(item.transform.position);
                tempHitPoint = item.transform.position;//物体被鼠标移动前的位置
                ray = Camera.main.ScreenPointToRay(itemScreenPos + deltaPos);
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(layerMask)))
            {
                if (Vector3.Distance(tempHitPoint, hit.point) > 0.001f)//鼠标是否在拖动物体移动（拒绝鼠标按下不松不移动可是物体还颤抖）
                {
                    item.transform.position = hit.point;
                }
            }
            else
            {
                Vector3 MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, SpaceScreen.z + Depth_Z);
                Vector3 TargetPos = Camera.main.ScreenToWorldPoint(MousePos);
                item.transform.position = TargetPos;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void ResetLastPos()
    {
        if (Input.GetMouseButtonUp(0))
        {
            lastPos = Vector3.zero;
        }
    }

    /// <summary>
    /// 获得烟花的钱数
    /// </summary>
    /// <param name="pos"></param>
    public void GetFireWorkIncome()
    {
        foreach (var item in GameManager.instance.firePlaneManager.firePlanes)
        {
            if (item.fireWork)
            {
                particleSystem = item.fireWork.GetComponentInChildren<ParticleSystem>();
                if (particleSystem.time > particleSystem.main.duration - 0.5f)
                {
                    if (PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1) == G.dc.gd.achievementTables.Length + 1)
                    {
                        isachieve = true;
                    }
                    else
                    {
                        if(isfinish == false)
                        {
                            launcher += 1;
                            PlayerPrefs.SetInt(G.ACHIEVEMENT, launcher);
                        }
                        
                        stage = Mathf.Clamp(PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1), G.dc.gd.achievementTables[0].level, G.dc.gd.achievementTableDict[G.dc.gd.achievementTables.Length].level);
                        if (PlayerPrefs.GetInt(G.ACHIEVEMENT, launcher) >= G.dc.gd.achievementTableDict[stage].accumulatelauncher)
                        {
                            isfinish = true;
                            GameManager.instance.playUI.launchRewardButton.gameObject.SetActive(true);
                            GameManager.instance.playUI.UnRewardLaunch.gameObject.SetActive(false);
                           

                        }
                        else
                        {
                            GameManager.instance.playUI.isreward = false;
                            GameManager.instance.playUI.launchRewardButton.gameObject.SetActive(false);
                            GameManager.instance.playUI.UnRewardLaunch.gameObject.SetActive(true);
                        }
                    }
                    
                    if (isachieve == true)
                    {
                        //Debug.Log("现在的等级是多少" + PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1));
                        if (PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1) == G.dc.gd.achievementTables.Length + 1)
                        {
                            GameManager.instance.AddMoney(item.fireWork.GetComponent<FireWork>().curFireworkIcome * G.dc.gd.achievementTableDict[PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1) - 1].multiple);
                            GameSceneManager.Instance.sceneCanvas.ShowMoneyText(item.fireWork.transform.position + Vector3.up, item.fireWork.GetComponent<FireWork>().curFireworkIcome * G.dc.gd.achievementTableDict[PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1) - 1].multiple);
                        }
                        else
                        {
                            int num = Mathf.Clamp(PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, 1), G.dc.gd.achievementTables[0].level, G.dc.gd.achievementTableDict[G.dc.gd.achievementTables.Length].level);
                            GameManager.instance.AddMoney(item.fireWork.GetComponent<FireWork>().curFireworkIcome * G.dc.gd.achievementTableDict[num-1].multiple);
                            GameSceneManager.Instance.sceneCanvas.ShowMoneyText(item.fireWork.transform.position + Vector3.up, item.fireWork.GetComponent<FireWork>().curFireworkIcome * G.dc.gd.achievementTableDict[num-1].multiple);
                        }
                    }
                    else
                    if (isachieve == false)
                    {
                        GameManager.instance.AddMoney(item.fireWork.GetComponent<FireWork>().curFireworkIcome);
                        GameSceneManager.Instance.sceneCanvas.ShowMoneyText(item.fireWork.transform.position + Vector3.up, item.fireWork.GetComponent<FireWork>().curFireworkIcome);
                    }
                    
                   
                    GameManager.instance.playUI.UpdateLauncherNumber(PlayerPrefs.GetInt(G.ACHIEVEMENTSTAGE, stage));
                    particleSystem.time = 0;
                }
            }
        }
        
    }

    
    public void ChangPosition(Vector3 pos)
    {
        Vector3 temp;
        temp = pos;
        pos = fireWork.transform.position;
        fireWork.transform.position = temp;
    }

    public void AddFireWork()
    {
        AudioManager.instance?.Tap();
        times = PlayerPrefs.GetInt(G.FIREWORKTIMES, 0);
        fireWorkLevel = PlayerPrefs.GetInt(G.FIREWORKLEVEL, 2);
        fireWorkLevel = Mathf.Clamp(fireWorkLevel, G.dc.gd.addFireWorkDatas[0].level, G.dc.gd.addFireWorkDatas[G.dc.gd.addFireWorkDatas.Length - 1].level);
        if (G.dc.GetMoney() >= G.dc.gd.addFireWorkDataDict[fireWorkLevel].cost)
        {
            for (int i = 0; i < GameManager.instance.preparePlaneManager.preparePlanes.Count; i++)
            {
                int level = PlayerPrefs.GetInt("FireWorkLevel" + GameManager.instance.preparePlaneManager.preparePlanes[i].PreparePlaneID, 0);
                if (level == 0 && PlayerPrefs.GetInt("PrepareUnlock" + GameManager.instance.preparePlaneManager.preparePlanes[i].PreparePlaneID, G.dc.gd.preparePlaneTableDict[GameManager.instance.preparePlaneManager.preparePlanes[i].PreparePlaneID].isunlock) == 1)
                {
                    item1 = i;
                    break;
                }
                else
                {
                    item1 = 5;
                }
            }
            if (item1 < 5)
            {
                GameManager.instance.UseFireWorkMoney(fireWorkLevel);
                fireWorkLevel += 1;
                PlayerPrefs.SetInt(G.FIREWORKLEVEL, fireWorkLevel);
                GameManager.instance.IsEnoughMoney();
                cub = Instantiate(firework, GameManager.instance.preparePlaneManager.preparePlanes[item1].transform.position, Quaternion.identity);
                cub.transform.parent = CameraManager.Instance.prepareRoot.transform;
                GameManager.instance.preparePlaneManager.preparePlanes[item1].fireWork = cub.GetComponent<FireWork>();
                int fireworklevel = G.dc.gd.levelDict[PlayerPrefs.GetInt(G.MAP, 1)].firelevel;
                cub.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[fireworklevel].level;
                cub.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[fireworklevel].income;
                cub.GetComponent<FireWork>().ShowModel(fireworklevel);
                cub.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                // GameManager.instance.fireworkUI.ShowUI(1);
                //PlayerPrefs.SetInt("Rocket" + 1,1);
                PlayerPrefs.SetInt("FireWorkLevel" + GameManager.instance.preparePlaneManager.preparePlanes[item1].PreparePlaneID, G.dc.gd.fireWorkDataDict[fireworklevel].level);
                //Debug.Log("FireWorkLevel" + GameManager.instance.preparePlaneManager.preparePlanes[item1].PreparePlaneID);
                //fireworkNum.Add(cub);
                PlayerPrefs.SetInt(G.FIREWORKTIMES, times + 1);
                GameManager.instance.playUI.UpdateNextmap(PlayerPrefs.GetInt(G.MAP,1));
            }
            else
            {
                Debug.LogError("准备台已满或者有准备台未解锁，无法继续添加");
            }
        }
        else
        {
            Debug.LogError("钱不够，无法继续添加");
        }      
    }
    public void AddIncome()
    {
        addIncomelevel = PlayerPrefs.GetInt(G.INCOME, 2);
        addIncomelevel = Mathf.Clamp(addIncomelevel, G.dc.gd.addIncomeDatas[0].level, G.dc.gd.addIncomeDatas[G.dc.gd.addIncomeDatas.Length - 1].level);
        if (G.dc.GetMoney() >= G.dc.gd.AddIncomeDataDict[addIncomelevel].cost)
        {
            addIncomelevel += 1;
            PlayerPrefs.SetInt(G.INCOME, addIncomelevel);
            GameManager.instance.UseIncomeMoney(PlayerPrefs.GetInt(G.INCOME, addIncomelevel));
            Debug.Log(PlayerPrefs.GetInt(G.INCOME, addIncomelevel));
        }
        else
        {
            Debug.LogError("钱不够，无法继续升级");
        }
    }
    public void ShowOrHideSlide()
    {

        if (PlayerPrefs.GetInt(G.STAGE, 1) == 1)
        {
            slide[0].SetActive(true);
            slide[1].SetActive(false);
            slide[2].SetActive(false);
            slide[3].SetActive(false);
            slide[4].SetActive(false);
        }
        else if (PlayerPrefs.GetInt(G.STAGE, 1) == 2)
        {
            slide[0].SetActive(false);
            slide[1].SetActive(true);
            slide[2].SetActive(false);
            slide[3].SetActive(false);
            slide[4].SetActive(false);
        }
        else if (PlayerPrefs.GetInt(G.STAGE, 1) == 3)
        {
            slide[0].SetActive(false);
            slide[1].SetActive(false);
            slide[2].SetActive(true);
            slide[3].SetActive(false);
            slide[4].SetActive(false);
        }
        else if (PlayerPrefs.GetInt(G.STAGE, 1) == 4)
        {
            slide[0].SetActive(false);
            slide[1].SetActive(false);
            slide[2].SetActive(false);
            slide[3].SetActive(true);
            slide[4].SetActive(false);
        }
        else if (PlayerPrefs.GetInt(G.STAGE, 1) == 5)
        {
            slide[0].SetActive(false);
            slide[1].SetActive(false);
            slide[2].SetActive(false);
            slide[3].SetActive(false);
            slide[4].SetActive(true);
        }
    }
    
}
