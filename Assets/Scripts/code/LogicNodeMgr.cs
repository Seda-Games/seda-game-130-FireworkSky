using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 逻辑节点管理器(逻辑树的根节点)，单例
/// </summary>
public class LogicNodeMgr : MonoBehaviour 
{
    private static LogicNodeMgr instance;
    public static LogicNodeMgr Instance
    {
        get
        {
            if (instance == null)
            {
                // 查找场景中的存在
                instance = FindObjectOfType<LogicNodeMgr>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<LogicNodeMgr>();
                    obj.name = instance.GetType().ToString();
                }
            }
            return instance;
        }
    }
    private static LogicNode newsNode = new LogicNode();

    public static bool IsInitialized
    {
        get { return instance != null; }
    }
    private void Update()
    {
        newsNode.OnLogicFrameRoot();
    }
    public static void SendEvent(uint eventKey, params object[] paramList)
    {
        newsNode.SendEvent(eventKey, paramList);
    }
    public static void SendEventNow(uint eventKey, params object[] paramList)
    {
        newsNode.SendEventNow(eventKey, paramList);
    }
    public static void AttachListener(uint eventKey, IEventListener listener)
    {
        newsNode.AttachListener(eventKey, listener);
    }
}
