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
    private Vector3 tempHitPoint;//�����ƶ�ǰ��λ��
    public string layerMask;//�ƶ�����
    public float Depth_Z;//����ƽ�����ƶ����������������λ�����
    bool is_element = false;
    public GameObject element;
    public GameObject element2;
    public GameObject element3;
    public FireWork fireWork;
    public List<GameObject> fireworkNum;
    int item1;
    public GameObject cub;
    int currentSpawnIndex=0;
    public List<GameObject> gameObjects;
    public GameObject newfirework;
    public ParticleSystem particleSystem;
    void Awake()
    {
        //InitFireWork();
        

    }
    // Start is called before the first frame update

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        ResetLastPos();
        ItemMove();
        GetFireWorkIncome();
    }


    public void ItemMove()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, int.MaxValue, 1 << Layer.Plane))
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
        }

        if (Input.GetMouseButton(0))//��ʼ�ƶ�
        {
            
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].GetComponent<BoxCollider>().enabled = false;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,Mathf.Infinity, 1 << Layer.Fire))
            {
                Debug.Log("ddddddddddddddddddddddddddddddddddddddd");
                if (hit.transform.CompareTag(Tag.FireWork))
                {
                    element = hit.collider.gameObject;
                    
                    element.layer = LayerMask.NameToLayer("Ignore Raycast");//���岻������ߣ����⵲ס������ƶ���
                    StartCoroutine(ItemMove(element));
                    Debug.Log("�ƶ�������");
                    
                }
                Debug.Log("1");
                //Debug.Log(hit.transform.gameObject);
            }

        }
        if (Input.GetMouseButtonUp(0))//ֹͣ�ƶ�
        {
            /*if (element)
            {
                element.layer = LayerMask.NameToLayer("Fire");//����ص���ʼ��
                Debug.Log(element.layer);
            }*/


            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].GetComponent<BoxCollider>().enabled = true;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, int.MaxValue, 1 << Layer.Plane))
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10000, Color.red);
                Debug.Log("????????" + hit.transform.name);
                //element.layer = LayerMask.NameToLayer("Ignore Raycast");//���岻������ߣ����⵲ס������ƶ���
                //Debug.Log(element.layer);
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
                        }
                        
                        if (element2.tag == Tag.FirePlane)
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
                                    if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel == element2.GetComponent<FirePlane>().fireWork.curFireworkLevel)
                                    {
                                        //�������̻�
                                        newfirework = Instantiate(firework, element3.transform.position, Quaternion.identity);
                                        newfirework.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel + 1].level;
                                        newfirework.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel + 1].income;

                                        //����ԭ���Ķ���
                                        Destroy(element3.GetComponent<FirePlane>().fireWork.gameObject);
                                        Destroy(element2.GetComponent<FirePlane>().fireWork.gameObject);

                                        //�������̻�
                                        element3.GetComponent<FirePlane>().fireWork = newfirework.GetComponent<FireWork>();
                                        newfirework.GetComponent<FireWork>().ShowModel(newfirework.GetComponent<FireWork>().curFireworkLevel);
                                        newfirework.GetComponent<FireWork>().fwp = FireWorkPhase.Fire;
                                        newfirework.GetComponent<FireWork>().PlayFx(newfirework, FireWorkPhase.Fire);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, 0);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                        element2.GetComponent<FirePlane>().fireWork = null;
                                    }
                                    else
                                    if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel != element2.GetComponent<FirePlane>().fireWork.curFireworkLevel)
                                    {
                                        //����λ�� 
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
                        if (element2.tag == Tag.PreparePlane)
                        {
                            if(element3)
                            {
                                if (element3.GetComponent<FirePlane>().fireWork == null)
                                {
                                    element3.GetComponent<FirePlane>().fireWork = element2.GetComponent<PreparePlane>().fireWork;
                                    PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, 0);
                                    PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, element3.GetComponent<FirePlane>().fireWork.curFireworkLevel);
                                    element2.GetComponent<PreparePlane>().fireWork = null;
                                }
                                else
                                if (element3.GetComponent<FirePlane>().fireWork != null && element2.GetComponent<PreparePlane>().fireWork != null)
                                {
                                    if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel == element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel)
                                    {
                                        //�������̻�
                                        newfirework = Instantiate(firework, element3.transform.position, Quaternion.identity);
                                        newfirework.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel + 1].level;
                                        newfirework.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel + 1].income;

                                        //����ԭ���Ķ���
                                        Destroy(element3.GetComponent<FirePlane>().fireWork.gameObject);
                                        Destroy(element2.GetComponent<PreparePlane>().fireWork.gameObject);

                                        //�������̻�
                                        element3.GetComponent<FirePlane>().fireWork = newfirework.GetComponent<FireWork>();
                                        newfirework.GetComponent<FireWork>().ShowModel(newfirework.GetComponent<FireWork>().curFireworkLevel);
                                        newfirework.GetComponent<FireWork>().fwp = FireWorkPhase.Fire;
                                        newfirework.GetComponent<FireWork>().PlayFx(newfirework, FireWorkPhase.Fire);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, 0);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                        element2.GetComponent<PreparePlane>().fireWork = null;
                                    }
                                    else
                                    if (element3.GetComponent<FirePlane>().fireWork.curFireworkLevel != element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel)
                                    {
                                        //����λ��
                                        element2.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                        element3.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element2.transform.position;

                                        FireWork temp;
                                        temp = element2.GetComponent<PreparePlane>().fireWork;
                                        element2.GetComponent<PreparePlane>().fireWork = element3.GetComponent<FirePlane>().fireWork;
                                        element3.GetComponent<FirePlane>().fireWork = temp;


                                        element2.GetComponent<PreparePlane>().fireWork.fwp = FireWorkPhase.Prepare;
                                        element2.GetComponent<PreparePlane>().fireWork.PlayFx(element2.GetComponent<PreparePlane>().fireWork.gameObject, FireWorkPhase.Prepare);

                                        element3.GetComponent<FirePlane>().fireWork.fwp = FireWorkPhase.Fire;
                                        element3.GetComponent<FirePlane>().fireWork.PlayFx(element3.GetComponent<FirePlane>().fireWork.gameObject, FireWorkPhase.Fire);



                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);
                                    }
                                }

                            }
                           
                        }

                        element = null;
                        element2 = null;
                        element3 = null;
                    }
                    Debug.Log("��⵽�ڷ���̨");
                }
                else
                if (hit.transform.CompareTag(Tag.PreparePlane))
                {
                    element3 = hit.collider.gameObject;
                    
                    Debug.Log("���������ʲô" + element3);
                    if (element2.transform.position == element3.transform.position)
                    {
                        if (element)
                        {
                            element.GetComponent<FireWork>().fwp = FireWorkPhase.Prepare;
                            element.GetComponent<FireWork>().PlayFx(element, FireWorkPhase.Prepare);
                            element.transform.position = element2.transform.position;
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
                        }
                        
                        if (element2.tag == Tag.FirePlane)
                        {//elemen3��PreparePlane,element2��FirePlane
                            if (element3)
                            {
                                if (element3.GetComponent<PreparePlane>().fireWork == null) 
                                {
                                    element3.GetComponent<PreparePlane>().fireWork = element2.GetComponent<FirePlane>().fireWork;
                                    PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, 0);
                                    PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel);
                                    element2.GetComponent<FirePlane>().fireWork = null;
                                }
                                if (element3.GetComponent<PreparePlane>().fireWork != null && element2.GetComponent<FirePlane>().fireWork != null)
                                {
                                    if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel == element2.GetComponent<FirePlane>().fireWork.curFireworkLevel)
                                    {
                                        //�������̻�
                                        newfirework = Instantiate(firework, element3.transform.position, Quaternion.identity);
                                        newfirework.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel + 1].level;
                                        newfirework.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel + 1].income;

                                        //����ԭ���Ķ���
                                        Destroy(element3.GetComponent<PreparePlane>().fireWork.gameObject);
                                        Destroy(element2.GetComponent<FirePlane>().fireWork.gameObject);

                                        //�������̻�
                                        element3.GetComponent<PreparePlane>().fireWork = newfirework.GetComponent<FireWork>();
                                        newfirework.GetComponent<FireWork>().ShowModel(newfirework.GetComponent<FireWork>().curFireworkLevel);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, 0);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                        element2.GetComponent<FirePlane>().fireWork = null;
                                    }
                                    else
                                    if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel != element2.GetComponent<FirePlane>().fireWork.curFireworkLevel)
                                    {
                                        //����λ��
                                        element2.GetComponent<FirePlane>().fireWork.gameObject.transform.position = element3.transform.position;
                                        element3.GetComponent<PreparePlane>().fireWork.gameObject.transform.position = element2.transform.position;
                                        

                                        FireWork temp;
                                        temp = element2.GetComponent<FirePlane>().fireWork;
                                        element2.GetComponent<FirePlane>().fireWork = element3.GetComponent<PreparePlane>().fireWork;
                                        element3.GetComponent<PreparePlane>().fireWork = temp;

                                        element2.GetComponent<FirePlane>().fireWork.fwp = FireWorkPhase.Fire;
                                        element2.GetComponent<FirePlane>().fireWork.PlayFx(element2.GetComponent<FirePlane>().fireWork.gameObject, FireWorkPhase.Fire);

                                        element3.GetComponent<PreparePlane>().fireWork.fwp = FireWorkPhase.Prepare;
                                        element3.GetComponent<PreparePlane>().fireWork.PlayFx(element3.GetComponent<PreparePlane>().fireWork.gameObject, FireWorkPhase.Prepare);




                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<FirePlane>().FirePlaneID, G.dc.gd.fireWorkDataDict[element2.GetComponent<FirePlane>().fireWork.curFireworkLevel].level);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel].level);
                                    }
                                }


                            }
                            
                        }
                        else
                        if (element2.tag == Tag.PreparePlane)
                        {//elemen3��PreparePlane,element2��PreparePlane
                            if (element3)
                            {
                                if (element3.GetComponent<PreparePlane>().fireWork== null)
                                {
                                    element3.GetComponent<PreparePlane>().fireWork = element2.GetComponent<PreparePlane>().fireWork;
                                    PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, 0);
                                    PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel);
                                    element2.GetComponent<PreparePlane>().fireWork = null;
                                }
                                if (element3.GetComponent<PreparePlane>().fireWork != null && element2.GetComponent<PreparePlane>().fireWork!=null)
                                {
                                    if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel == element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel)
                                    {
                                        //�������̻�
                                        newfirework =Instantiate(firework, element3.transform.position, Quaternion.identity);
                                        newfirework.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel + 1].level;
                                        newfirework.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel + 1].income;

                                        //����ԭ���Ķ���
                                        Destroy(element3.GetComponent<PreparePlane>().fireWork.gameObject);
                                        Destroy(element2.GetComponent<PreparePlane>().fireWork.gameObject);
                                        
                                        //�������̻�
                                        element3.GetComponent<PreparePlane>().fireWork = newfirework.GetComponent<FireWork>();
                                        newfirework.GetComponent<FireWork>().ShowModel(newfirework.GetComponent<FireWork>().curFireworkLevel);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element2.GetComponent<PreparePlane>().PreparePlaneID, 0);
                                        PlayerPrefs.SetInt("FireWorkLevel" + element3.GetComponent<PreparePlane>().PreparePlaneID, G.dc.gd.fireWorkDataDict[newfirework.GetComponent<FireWork>().curFireworkLevel].level);
                                        element2.GetComponent<PreparePlane>().fireWork = null;

                                        
                                    }
                                    else
                                    if (element3.GetComponent<PreparePlane>().fireWork.curFireworkLevel != element2.GetComponent<PreparePlane>().fireWork.curFireworkLevel)
                                    {//����λ��
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
                        element = null;
                        element2 = null;
                        element3 = null;
                    }
                    Debug.Log("��⵽��׼��̨");
                }
                else
                if (!hit.transform.CompareTag(Tag.FirePlane) && !hit.transform.CompareTag(Tag.PreparePlane))
                {
                    //Debug.Log(hit.transform.gameObject.layer);
                    //Debug.Log(hit.transform.tag);
                    Debug.Log(element.layer);
                    Debug.Log(element.tag);
                    element.transform.position = element2.transform.position;
                    element = null;
                    element2 = null;
                    element3 = null;

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
            
        }
        if (element)
        {
            element.layer = LayerMask.NameToLayer("Fire");//����ص���ʼ��
        }

    }

    public IEnumerator ItemMove(GameObject item)
    {
        Vector3 SpaceScreen = Camera.main.WorldToScreenPoint(Camera.main.transform.position);//��Ļ���ƶ������
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
                tempHitPoint = item.transform.position;//���屻����ƶ�ǰ��λ��
                ray = Camera.main.ScreenPointToRay(itemScreenPos + deltaPos);
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(layerMask)))
            {
                if (Vector3.Distance(tempHitPoint, hit.point) > 0.001f)//����Ƿ����϶������ƶ����ܾ���갴�²��ɲ��ƶ��������廹������
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
    /// ����
    /// </summary>
    public void ResetLastPos()
    {
        if (Input.GetMouseButtonUp(0))
        {
            lastPos = Vector3.zero;
        }
    }

    /// <summary>
    /// ����̻���Ǯ��
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
                    GameManager.instance.AddMoney(item.fireWork.GetComponent<FireWork>().curFireworkIcome);
                    particleSystem.time = 0;
                }
            }
        }
        
    }

    #region ��ʱ����
    public void FireWorkStart(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            element = hit.collider.gameObject;
            if (hit.transform.CompareTag(Tag.FireWork))
            {
                element = hit.collider.gameObject;
                element.layer = LayerMask.NameToLayer("Ignore Raycast");//���岻������ߣ����⵲ס������ƶ���
                Debug.Log("�ҵ�����̻�");
            }
            /*else
            if (hit.transform.CompareTag(Tag.PreparePlane))
            {
                PreparePlane preparePlane = element.GetComponent<PreparePlane>();
                preparePlane.fireWork = null;
                Debug.Log("�ҵ����׼��ƽ̨");
            }
            else
            if (hit.transform.CompareTag(Tag.FirePlane))
            {
                FirePlane firePlane = element.GetComponent<FirePlane>();
                firePlane.fireWork = null;
                Debug.Log("�ҵ���˷���ƽ̨");
            }*/
        }
    }
    public void FireWorkMove(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag(Tag.FireWork))
            {
                is_element = true;
                element = hit.collider.gameObject;
                originalPosition = element.transform.position;
                fireWork = element.GetComponent<FireWork>();
                //element.layer = LayerMask.NameToLayer("Ignore Raycast");//���岻������ߣ����⵲ס������ƶ���
                Vector3 SpaceScreen = Camera.main.WorldToScreenPoint(Camera.main.transform.position);//��Ļ���ƶ������
                if (element)
                {
                    currentPos = pos;
                    if (lastPos != Vector3.zero)
                    {
                        deltaPos = currentPos - lastPos;
                    }

                    lastPos = currentPos;
                    Vector3 itemScreenPos = Camera.main.WorldToScreenPoint(element.transform.position);
                    tempHitPoint = element.transform.position;//���屻����ƶ�ǰ��λ��
                    ray = Camera.main.ScreenPointToRay(itemScreenPos + deltaPos);
                }
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(layerMask)))
                {
                    if (Vector3.Distance(tempHitPoint, hit.point) > 0.001f)//����Ƿ����϶������ƶ����ܾ���갴�²��ɲ��ƶ��������廹������
                    {
                        element.transform.position = hit.point;
                    }
                }
                else
                {
                    Vector3 MousePos = new Vector3(pos.x, pos.y, SpaceScreen.z + Depth_Z);
                    Vector3 TargetPos = Camera.main.ScreenToWorldPoint(MousePos);
                    element.transform.position = TargetPos;
                }

                element.layer=LayerMask.NameToLayer("Default");
            }
        }
    }

    public void FireWorkMoveEnd(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag(Tag.FirePlane))
            {
                element=hit.collider.gameObject;

                FirePlane firePlane = element.GetComponent<FirePlane>();
                if (firePlane.fireWork == null)
                {
                    //ChangPosition();
                }
                if (firePlane.fireWork !=null)
                {
                    if (firePlane.fireWork.curFireworkLevel == fireWork.curFireworkLevel)
                    {
                        Combine(element,element2);
                    }
                    else
                    if (firePlane.fireWork.curFireworkLevel != fireWork.curFireworkLevel)
                    {
                        Vector3 firepositon = firePlane.fireWork.transform.position;
                        ChangPosition(firepositon);
                    }
                    
                }
            }
            else
            if (hit.transform.CompareTag(Tag.PreparePlane))
            {
                element = hit.collider.gameObject;
                PreparePlane preparePlane = element.GetComponent<PreparePlane>();
                if (preparePlane.fireWork == null)
                {
                    //ChangPosition();
                }
                else
                if (preparePlane.fireWork != null)
                {
                    if (preparePlane.fireWork.curFireworkLevel == fireWork.curFireworkLevel)
                    {
                        Combine(element,element2);
                    }
                    else
                    if (preparePlane.fireWork.curFireworkLevel != fireWork.curFireworkLevel)
                    {
                        Vector3 prepositon = preparePlane.fireWork.transform.position;
                        ChangPosition(prepositon);
                    }
                }
            }
            else
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward) * 1000;
                Debug.DrawRay(transform.position,forward,Color.green);
                Debug.Log(hit.transform.tag);
                element.transform.position = originalPosition;
            }
        }
    }

    #endregion
    public void ChangPosition(Vector3 pos)
    {
        Vector3 temp;
        temp = pos;
        pos = fireWork.transform.position;
        fireWork.transform.position = temp;
    }
    public void Combine(GameObject obj,GameObject obj1)
    {
        
    }
    public void InitFireWork()
    {
        foreach (var item in fireworkNum)
        {
            
        }
        /*foreach (var item in GameManager.instance.firePlaneManager.firePlanes)
        {
            int id = item.FirePlaneID;
            Transform pos = item.transform.GetComponent<Transform>();
            bool isshow = item.isexist;
            int level = item.fireWork.curFireworkLevel;
            int income = item.fireWork.curFireworkIcome;
            Debug.Log("id" + id);
            Debug.Log("pos" + pos);
            Debug.Log("isshow" + isshow);
            Debug.Log("level" + level);
            Debug.Log("income" + income);

        }
        foreach (var item in GameManager.instance.preparePlaneManager.preparePlanes)
        {
            int id = item.PreparePlaneID;
            Transform pos = item.transform.GetComponent<Transform>();
            bool isshow = item.isexist;
            int level = item.fireWork.curFireworkLevel;
            int income = item.fireWork.curFireworkIcome;
            Debug.Log("id" + id);
            Debug.Log("pos" + pos);
            Debug.Log("isshow" + isshow);
            Debug.Log("level" + level);
            Debug.Log("income" + income);
        }*/
    }
    public void AddFireWork()
    {
        //var item1= GameManager.instance.preparePlaneManager.preparePlanes;
        /*foreach (var item in GameManager.instance.preparePlaneManager.preparePlanes)
        {
            int level = PlayerPrefs.GetInt("FireWorkLevel" + item.PreparePlaneID, 0);
            if (level == 0)
            {
                //item1 = item;
            }
        }*/
        AudioManager.instance?.Tap();
        int curlevel = PlayerPrefs.GetInt(G.LEVEL, 1);
        if (G.dc.GetMoney() >= G.dc.gd.levelDict[curlevel].fireworkcost)
        {
            for (int i = 0; i < GameManager.instance.preparePlaneManager.preparePlanes.Count; i++)
            {
                int level = PlayerPrefs.GetInt("FireWorkLevel" + GameManager.instance.preparePlaneManager.preparePlanes[i].PreparePlaneID, 0);
                if (level == 0)
                {
                    item1 = i;
                    break;
                }
                else
                {
                    item1 = 3;
                }
            }
            if (item1 < 3)
            {

                cub = Instantiate(firework, GameManager.instance.preparePlaneManager.preparePlanes[item1].transform.position, Quaternion.identity);
                GameManager.instance.preparePlaneManager.preparePlanes[item1].fireWork = cub.GetComponent<FireWork>();
                cub.GetComponent<FireWork>().curFireworkLevel = G.dc.gd.fireWorkDataDict[1].level;
                cub.GetComponent<FireWork>().curFireworkIcome = G.dc.gd.fireWorkDataDict[1].income;
                cub.GetComponent<FireWork>().ShowModel(1);
                PlayerPrefs.SetInt("FireWorkLevel" + GameManager.instance.preparePlaneManager.preparePlanes[item1].PreparePlaneID, G.dc.gd.fireWorkDataDict[1].level);
                Debug.Log("FireWorkLevel" + GameManager.instance.preparePlaneManager.preparePlanes[item1].PreparePlaneID);
                //fireworkNum.Add(cub);
                GameManager.instance.UseMoney(curlevel);

            }
            else
            {
                Debug.LogError("׼��̨�������޷��������");
            }
        }
                   
                
    }
    public void AddFireWorkIncome()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tag.FireWork)
        {

        }
    }
    
    
}
