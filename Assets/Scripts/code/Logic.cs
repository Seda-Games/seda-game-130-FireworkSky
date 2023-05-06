
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 帧逻辑
/// </summary>
public interface ILogic
{
    void OnFixedUpdate();
    void OnUpdate();
    void OnLateUpdate();
}

/// <summary>
/// 消息
/// </summary>
public interface IEvent
{
    uint GetKey();
    object GetParam1();
    object GetParam2();

    object[] GetParamList();
}

/// <summary>
/// 
/// </summary>
public class GameEvent : IEvent
{
    private uint key;
    private object param1 = null;
    private object param2 = null;
    private object[] paramList = null;

    public GameEvent(uint key, object param1, object param2)
    {
        this.key = key;
        this.param1 = param1;
        this.param2 = param2;
    }

    public GameEvent(uint key, params object[] paramList)
    {
        this.key = key;
        this.paramList = paramList;
    }

    public uint GetKey()
    {
        return key;
    }

    public object GetParam1()
    {
        return param1;
    }

    public object GetParam2()
    {
        return param2;
    }

    public object[] GetParamList()
    {
        return paramList;
    }
}

/// <summary>
/// 消息接口
///     消息机制的接口
/// </summary>
public interface IEventListener
{
    /// <summary>
    /// 消息处理
    /// </summary>
    /// <param name="key"></param>
    /// <param name="paramList"></param>
    /// <returns></returns>
    bool HandleEvent(uint key, params object[] paramList);

    /// <summary>
    /// 返回消息监听器针对消息id的优先级
    /// 用于针对不同消息id对本监听器设置不同的监听优先级
    /// 数值越大优先级越高
    /// 消息优先级排序只会在AttachListener的时候发生一次
    /// </summary>
    /// <returns></returns>
    int GetListenerPriority(uint eventKey);
}

/// <summary>
/// 帧逻辑节点
/// </summary>
public class LogicNode : MonoBehaviour, ILogic
{
    #region 外部接口

    public int NodePriority { get { return m_iNodePriority; } set { m_iNodePriority = value; } }

    /// <summary>
    /// 消息派送
    /// </summary>
    public bool EventDispatcher { get { return m_bIsEventDispatcher; } set { m_bIsEventDispatcher = value; } }

    /// <summary>
    /// 挂载逻辑节点
    /// </summary>
    /// <param name="node"></param>
    public void AttachLogicNode(LogicNode node)
    {
        // 如果节点树在更新了，则节点先进队列，等待下一帧一开始加入逻辑树
        // 否则直接添加
        if(m_bIsInProcessing)
        {
            m_logicNodeToUpdate.Enqueue(new NodePack(node, true));
        }
        else
        {
            AttachLogicNode_Now(node);
        }
    }

    /// <summary>
    /// 卸载逻辑节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public void DetachLogicNode(LogicNode node)
    {
        // 如果节点树在更新了，则节点先进队列，等待下一帧开始从逻辑树中移除
        // 否则直接移除
        if (m_bIsInProcessing)
        {
            m_logicNodeToUpdate.Enqueue(new NodePack(node, false));
        }
        else
        {
            DetachLogicNode_Now(node);
        }
    }

    /// <summary>
    /// 挂载逻辑节点
    /// </summary>
    public void AttachLogic(ILogic logic, LogicType type = LogicType.Default, int priority = 0)
    {
        if (m_bIsInProcessing)
        {
            m_logicToUpdate.Enqueue(new LogicPack(new LogicLeaf(logic, priority, type), true));
        }
        else
        {
            AttachLogic_Now(new LogicLeaf(logic, priority, type));
        }
    }

    /// <summary>
    /// 卸载逻辑节点
    /// </summary>
    public void DetachLogic(ILogic logic)
    {
        if (m_bIsInProcessing)
        {
            // 构造一个新的逻辑叶子，属性默认，只用来包装 ILogic
            m_logicToUpdate.Enqueue(new LogicPack(new LogicLeaf(logic), false));
        }
        else
        {
            DetachLogic_Now(logic);
        }
    }

    /// <summary>
    /// 挂载一个消息监听到当前消息节点
    /// </summary>
    /// <param name="eventKey">消息码</param>
    /// <param name="listener">消息监听器</param>
    /// <returns></returns>
    public void AttachListener(uint eventKey, IEventListener listener)
    {
        if (m_bIsInProcessing)
        {
            m_listenerToUpdate.Enqueue(new ListenerPack(eventKey, listener, true));
        }
        else
        {
            AttachListener_Now(eventKey,listener);
        }
    }

    /// <summary>
    /// 卸载一个消息监听
    /// </summary>
    /// <param name="eventKey"></param>
    /// <param name="listener"></param>
    /// <returns></returns>
    public void DetachListener(uint eventKey, IEventListener listener)
    {
        if (m_bIsInProcessing)
        {
            m_listenerToUpdate.Enqueue(new ListenerPack(eventKey, listener, true));
        }
        else
        {
            DetachListener_Now(eventKey, listener);
        }
    }

    /// <summary>
    /// 发送一个消息事件，进入消息队列，在下一帧执行派送
    /// </summary>
    /// <param name="eventKey"></param>
    /// <param name="paramList"></param>
    public void SendEvent(uint eventKey, params object[] paramList)
    {
        eventQueue_Next.Enqueue(new GameEvent(eventKey, paramList));
    }

    /// <summary>
    /// 立即执行消息派送
    /// </summary>
    /// <param name="eventKey"></param>
    /// <param name="paramList"></param>
    public void SendEventNow(uint eventKey, params object[] paramList)
    {
        DispatchEvent(eventKey, paramList);
    }

    #endregion

    #region 根逻辑节点执行

    /// <summary>
    /// 逻辑根节点专用方法
    /// 用于从逻辑根上发起一次逻辑帧
    /// FixedUpdate,Update,LateUpdate等调用均可
    /// 必须主线程调用
    /// </summary>
    public void OnLogicFrameRoot()
    {
        m_bIsInProcessing = true;
        if (gameObject.activeInHierarchy && enabled)
        {
            // 更新节点树
            OnUpdateNodeTree();
            // 派送消息
            DispatchEventQueue();
        }
        m_bIsInProcessing = false;
    }

    public void OnFixedUpdateRoot()
    {
        m_bIsInProcessing = true;
        if(gameObject.activeSelf && this.enabled)
        {
            OnFixedUpdate();
        }
        m_bIsInProcessing = false;
    }

    public void OnUpdateRoot()
    {
        m_bIsInProcessing = true;
        if (gameObject.activeSelf && this.enabled)
        {
            OnUpdate();
        }
        m_bIsInProcessing = false;
    }

    public void OnLateUpdateRoot()
    {
        m_bIsInProcessing = true;
        if (gameObject.activeSelf && this.enabled)
        {
            OnLateUpdate();
        }
        m_bIsInProcessing = false;
    }

    #endregion

    #region 方法实现

    /// <summary>
    /// 是否在运行中
    /// </summary>
    private bool m_bIsInProcessing = false;
    /// <summary>
    /// 节点 优先级
    /// </summary>
    private int m_iNodePriority = 0;
    /// <summary>
    /// 是否激活该节点的消息派送
    /// </summary>
    private bool m_bIsEventDispatcher = true;

    /// <summary>
    /// 更新整个节点树
    /// </summary>
    private void OnUpdateNodeTree()
    {
        // 更新当前节点的
        UpdateLogicNodeList();
        UpdateLogicList();
        UpdateListenerList();

        // 更新子节点的
        int count = m_nodeList.Count;
        for(int i = 0; i < count; i++)
        {
            m_nodeList[i].OnUpdateNodeTree();
        }
    }

    #region 逻辑节点挂载和卸载
    /// <summary>
    /// 事件逻辑节点列表
    /// </summary>
    private List<LogicNode> m_nodeList = new List<LogicNode>();

    /// <summary>
    /// 用于在每逻辑帧开始的时候维护逻辑节点序列，分辨该节点为添加还是移除
    /// </summary>
    private struct NodePack
    {
        public LogicNode logicNode;
        public bool addOrRemove;

        public NodePack(LogicNode node, bool addOrRe)
        {
            logicNode = node;
            addOrRemove = addOrRe;
        }
    }

    /// <summary>
    /// 在游戏运行中添加或移除的逻辑节点，等待下一帧开始更新
    /// </summary>
    Queue<NodePack> m_logicNodeToUpdate = new Queue<NodePack>();

    /// <summary>
    /// 立即挂载逻辑节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private void AttachLogicNode_Now(LogicNode node)
    {
        if (node == null)
        {
            Debug.Log("AttachLogicNode_Now -> node is Null");
            return;
        }

        if (m_nodeList.Contains(node))
        {
            Debug.Log("AttachLogicNode_Now -> node is Exit");
            return;
        }

        int pos = 0;
        int count = m_nodeList.Count;
        for (int i = 0; i < count; i++)
        {
            if (node.NodePriority > m_nodeList[i].NodePriority)
            {
                break;
            }
            pos++;
        }
        m_nodeList.Insert(pos, node);
    }

    /// <summary>
    /// 立即卸载一个逻辑节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private void DetachLogicNode_Now(LogicNode node)
    {
        if (m_nodeList.Contains(node))
        {
            m_nodeList.Remove(node);
        }
    }

    /// <summary>
    /// 更新逻辑列表
    /// </summary>
    private void UpdateLogicNodeList()
    {
        _UpdateLogicNodeList();
    }

    /// <summary>
    /// 处理等待添加或移除的队列
    /// </summary>
    private void _UpdateLogicNodeList()
    {
        int count = m_logicNodeToUpdate.Count;

        while (count != 0)
        {
            count--;
            NodePack nodePack = m_logicNodeToUpdate.Dequeue(); // 取出一个逻辑节点缓存
            if (nodePack.addOrRemove)
            {
                AttachLogicNode_Now(nodePack.logicNode);
            }
            else
            {
                DetachLogicNode_Now(nodePack.logicNode);
            }
        }
    }
    #endregion

    #region 帧逻辑的挂载和卸载

    /// <summary>
    /// 封装帧逻辑，区分更新优先级
    /// </summary>
    private class LogicLeaf
    {
        public ILogic logic;
        public int logicPriority;
        public LogicType type;

        public LogicLeaf(ILogic logic, int priority = 0, LogicType type = LogicType.Default)
        {
            this.logic = logic;
            this.logicPriority = priority;
            this.type = type;
        }      
    }

    /// <summary>
    /// 帧逻辑更新类型
    /// </summary>
    public enum LogicType
    {
        Default,
        Update,
        FixedUpdate,
        LateUpdate
    }

    /// <summary>
    /// 用于在每逻辑帧开始的时候维护逻辑序列，分辨该逻辑为添加还是移除
    /// </summary>
    private struct LogicPack
    {
        public LogicLeaf logicLeaf;
        public bool addOrRemove;
        public LogicPack(LogicLeaf logicLeaf, bool addOrRemove)
        {
            this.logicLeaf = logicLeaf;
            this.addOrRemove = addOrRemove;
        }
    }

    Queue<LogicPack> m_logicToUpdate = new Queue<LogicPack>();

    /// <summary>
    /// 统一执行的逻辑列表
    /// </summary>
    private List<LogicPack> logicList;

    private List<LogicLeaf> logicOnFixedUpdateList;
    private List<LogicLeaf> logicOnUpdateList;
    private List<LogicLeaf> logicOnLateUpdateList;

    /// <summary>
    /// 更新逻辑列表
    /// </summary>
    private void UpdateLogicList()
    {
        _UpdateLogicList();
    }

    private void _UpdateLogicList()
    {
        int count = m_logicToUpdate.Count;

        while (count != 0)
        {
            count--;
            LogicPack logicPack = m_logicToUpdate.Dequeue(); // 取出一个逻辑缓存
            if (logicPack.addOrRemove)
            {
                AttachLogic_Now(logicPack.logicLeaf);
            }
            else
            {
                DetachLogic_Now(logicPack.logicLeaf.logic);
            }
        }
    }

    /// <summary>
    /// 存放逻辑与其位置的关系
    /// </summary>
    private Dictionary<ILogic, LogicLeaf> logciLeafDict = new Dictionary<ILogic, LogicLeaf>();

    private void AttachLogic_Now(LogicLeaf logicLeaf)
    {
        if(logicLeaf == null)
        {
            return;
        }

        if (logicLeaf.logic == null)
        {
            Debug.Log("LogicNode, AttachLogic_Now: failed due to logic null.");
            logicLeaf = null;
            return;
        }

        logciLeafDict.Add(logicLeaf.logic, logicLeaf);

        switch (logicLeaf.type)
        {
            case LogicType.Default:
                OnAttachUpdateLogic(logicLeaf);
                OnAttachFixedUpdateLogic(logicLeaf);
                OnAttachLateUpdateLogic(logicLeaf);
                break;
            case LogicType.Update:
                OnAttachUpdateLogic(logicLeaf);
                break;
            case LogicType.FixedUpdate:
                OnAttachFixedUpdateLogic(logicLeaf);
                break;
            case LogicType.LateUpdate:
                OnAttachLateUpdateLogic(logicLeaf);
                break;
        }
    }

    private void DetachLogic_Now(ILogic logic)
    {
        if(logciLeafDict.ContainsKey(logic))
        {
            LogicLeaf leaf = logciLeafDict[logic];
            logciLeafDict.Remove(logic);

            switch (leaf.type)
            {
                case LogicType.Default:
                    OnDetachFixedUpdateLogic(leaf);
                    OnDetachLateUpdateLogic(leaf);
                    OnDetachUpdateLogic(leaf);
                    break;
                case LogicType.Update:
                    OnDetachUpdateLogic(leaf);
                    break;
                case LogicType.FixedUpdate:
                    OnDetachFixedUpdateLogic(leaf);                    
                    break;
                case LogicType.LateUpdate:
                    OnDetachLateUpdateLogic(leaf);
                    break;
            }
        }
    }

    /// <summary>
    /// 挂载Update更新逻辑
    /// </summary>
    /// <param name="logicLeaf"></param>
    private void OnAttachUpdateLogic(LogicLeaf logicLeaf)
    {
        if(logicOnUpdateList == null)
        {
            logicOnUpdateList = new List<LogicLeaf>() { logicLeaf };
            return;
        }

        int pos = 0;
        int count = logicOnUpdateList.Count;
        for (int i = 0; i < count; i++)
        {
            if (logicLeaf.logicPriority > logicOnUpdateList[i].logicPriority)
            {
                break;
            }
            pos++;
        }
        logicOnUpdateList.Insert(pos, logicLeaf);
    }

    /// <summary>
    /// 挂载FixedUpdate更新逻辑
    /// </summary>
    /// <param name="logicLeaf"></param>
    private void OnAttachFixedUpdateLogic(LogicLeaf logicLeaf)
    {
        if (logicOnFixedUpdateList == null)
        {
            logicOnFixedUpdateList = new List<LogicLeaf>() { logicLeaf };
            return;
        }

        int pos = 0;
        int count = logicOnFixedUpdateList.Count;
        for (int i = 0; i < count; i++)
        {
            if (logicLeaf.logicPriority > logicOnFixedUpdateList[i].logicPriority)
            {
                break;
            }
            pos++;
        }
        logicOnFixedUpdateList.Insert(pos, logicLeaf);
    }

    /// <summary>
    /// 挂载LateUpdate更新逻辑
    /// </summary>
    /// <param name="logicLeaf"></param>
    private void OnAttachLateUpdateLogic(LogicLeaf logicLeaf)
    {
        if (logicOnLateUpdateList == null)
        {
            logicOnLateUpdateList = new List<LogicLeaf>() { logicLeaf };
            return;
        }

        int pos = 0;
        int count = logicOnLateUpdateList.Count;
        for (int i = 0; i < count; i++)
        {
            if (logicLeaf.logicPriority > logicOnLateUpdateList[i].logicPriority)
            {
                break;
            }
            pos++;
        }
        logicOnLateUpdateList.Insert(pos, logicLeaf);
    }

    /// <summary>
    /// 卸载Update帧逻辑
    /// </summary>
    /// <param name="logicLeaf"></param>
    private void OnDetachUpdateLogic(LogicLeaf logicLeaf)
    {
        if(logicOnUpdateList != null && logicOnUpdateList.Contains(logicLeaf))
        {
            logicOnUpdateList.Remove(logicLeaf);
        }
    }

    /// <summary>
    /// 卸载FixedUpdate帧逻辑
    /// </summary>
    /// <param name="logicLeaf"></param>
    private void OnDetachFixedUpdateLogic(LogicLeaf logicLeaf)
    {
        if (logicOnFixedUpdateList != null && logicOnFixedUpdateList.Contains(logicLeaf))
        {
            logicOnFixedUpdateList.Remove(logicLeaf);
        }
    }

    /// <summary>
    /// 卸载LateUpdate帧逻辑
    /// </summary>
    /// <param name="logicLeaf"></param>
    private void OnDetachLateUpdateLogic(LogicLeaf logicLeaf)
    {
        if (logicOnLateUpdateList != null && logicOnLateUpdateList.Contains(logicLeaf))
        {
            logicOnLateUpdateList.Remove(logicLeaf);
        }
    }

    #endregion

    #region 事件监听挂载和卸载，事件的发送

    /// <summary>
    /// 所有消息的列表
    /// </summary>
    private Dictionary<uint, List<IEventListener>> m_listenerList = new Dictionary<uint, List<IEventListener>>();

    private struct ListenerPack
    {
        public uint evenKey;
        public IEventListener listener;
        public bool addOrRemove;

        public ListenerPack(uint evenKey, IEventListener listener, bool addOrRemove)
        {
            this.evenKey = evenKey;
            this.listener = listener;
            this.addOrRemove = addOrRemove;
        }
    }

    /// <summary>
    /// 用于保存在逻辑帧开始的时候需要更新的ListenerPack
    /// </summary>
    private Queue<ListenerPack> m_listenerToUpdate = new Queue<ListenerPack>();

    /// <summary>
    /// 当前消息队列
    /// </summary>
    private Queue<IEvent> eventQueue_Now = new Queue<IEvent>();
    /// <summary>
    /// 下一个消息队列
    /// </summary>
    private Queue<IEvent> eventQueue_Next = new Queue<IEvent>();
    

    /// <summary>
    /// 更新监听器列表
    /// </summary>
    private void UpdateListenerList()
    {
        _UpdateListenerList();
    }

    private void _UpdateListenerList()
    {
        int count = m_listenerToUpdate.Count;

        while (count != 0)
        {
            count--;
            ListenerPack listenerPack = m_listenerToUpdate.Dequeue(); // 取出一个逻辑缓存
            if (listenerPack.addOrRemove)
            {
                AttachListener_Now(listenerPack.evenKey, listenerPack.listener);
            }
            else
            {
                DetachListener_Now(listenerPack.evenKey, listenerPack.listener);
            }
        }
    }

    /// <summary>
    /// 执行一个消息监听器的挂载
    /// </summary>
    /// <param name="listener"></param>
    /// <param name="eventKey"></param>
    private void AttachListener_Now(uint eventKey,IEventListener listener)
    {
        if (listener == null)
        {
            return;
        }

        // 不存在消息列表
        if (!m_listenerList.ContainsKey(eventKey))
        {
            // 新建一个消息列表
            m_listenerList.Add(eventKey, new List<IEventListener>() { listener });
            return;
        }


        // 该消息列表中是否已经挂载了，该消息监听器
        if (m_listenerList[eventKey].Contains(listener))
        {
            return;
        }

        int pos = 0;
        int count = m_listenerList[eventKey].Count;
        for (int i = 0; i < count; i++)
        {
            if (listener.GetListenerPriority(eventKey) > m_listenerList[eventKey][i].GetListenerPriority(eventKey))
            {
                break;
            }
            pos++;
        }

        m_listenerList[eventKey].Insert(pos, listener);
    }

    /// <summary>
    /// 执行一个消息监听器的卸载
    /// </summary>
    /// <param name="eventKey"></param>
    /// <param name="listener"></param>
    /// <returns></returns>
    private void DetachListener_Now(uint eventKey, IEventListener listener)
    {
        if (m_listenerList.ContainsKey(eventKey) && m_listenerList[eventKey].Contains(listener))
        {
            m_listenerList[eventKey].Remove(listener);
        }
    }


    /// <summary>
    /// 发送消息队列里的消息
    /// </summary>
    private void DispatchEventQueue()
    {
        // 该节点或节点挂载的对象未激活
        if (!m_bIsEventDispatcher || !this.gameObject.activeSelf || !this.enabled)
        {
            return;
        }
        int count = eventQueue_Now.Count;
        // 派送该节点消息监听器的消息队列
        while (count != 0)
        {
            count--;
            IEvent ev = eventQueue_Now.Dequeue();
            if(ev != null)
            {
                DispatchEvent(ev.GetKey(), ev.GetParamList());
            }
        }

        // 派送子节点的消息队列
        int nodeListCount = m_nodeList.Count;
        for (int i = 0; i < nodeListCount;i++)
        {
            m_nodeList[i].DispatchEventQueue();
        }

        SwicthEventQueue();
    }

    /// <summary>
    /// 切换下一个消息队列
    /// </summary>
    private void SwicthEventQueue()
    {
        Queue<IEvent> temp = eventQueue_Now;
        eventQueue_Now = eventQueue_Next;
        eventQueue_Next = temp;
    }

    /// <summary>
    /// 深度优先遍历
    /// 派发消息到子消息节点级自己节点小的监听器上
    /// </summary>
    /// <param name="key"></param>
    /// <param name="paramList"></param>
    /// <returns></returns>
    private bool DispatchEvent(uint key, params object[] paramList)
    {
        // 该节点或节点挂载的对象未激活
        if (!m_bIsEventDispatcher || !this.gameObject.activeSelf || !this.enabled)
        {
            return false;
        }

        int count = m_nodeList.Count;
        for (int i = 0; i < count; i++)
        {
            if (m_nodeList[i].DispatchEvent(key, paramList))
            {
                return true;
            }
        }

        return TriggerEvent(key, paramList);
    }

    /// <summary>
    /// 消息触发
    /// </summary>
    /// <param name="key"></param>
    /// <param name="paramList"></param>
    /// <returns></returns>
    private bool TriggerEvent(uint key, params object[] paramList)
    {       
        if (!m_listenerList.ContainsKey(key))
        {
            Debug.Log(this.GetType() + "(LogicNode)::TriggerEvent-> EventKey "+ key + " is not Exit!");
            return false;
        }

        List<IEventListener> listeners = m_listenerList[key];
        int count = listeners.Count;
        bool ret = true;
        for (int i = 0; i < count; i++)
        {
            if (listeners[i].HandleEvent(key, paramList))
            {
                ret = true;
            }
            else
            {
                Debug.Log(listeners[i].GetType() + " " + key + " is Error");
                ret = false;
            }
        }

        return ret;
    }

    #endregion

    #endregion

    #region 逻辑更新
    public virtual void OnFixedUpdate()
    {
        // 先更新该节点的子节点，在遍历了逻辑节点，深度优先遍历
        LogicNode node = null;
        int i = 0;
        int count = m_nodeList.Count;
        for(i = 0; i < count; i++)
        {
            node = m_nodeList[i];
            if(node.gameObject.activeSelf && node.enabled)
            {
                node.OnFixedUpdate();
            }
        }


        // 更新该节点的叶子节点
        if(logicOnFixedUpdateList!= null)
        {
            ILogic logic = null;
            count = logicOnFixedUpdateList.Count;
            for(i = 0; i < count; i++)
            {
                logic = logicOnFixedUpdateList[i].logic;
                logic.OnFixedUpdate();
            }
        }
        
    }
    public virtual void OnUpdate()
    {
        // 先更新该节点的子节点，在遍历了逻辑节点，深度优先遍历
        LogicNode node = null;
        int i = 0;
        int count = m_nodeList.Count;
        for (i = 0; i < count; i++)
        {
            node = m_nodeList[i];
            if (node.gameObject.activeSelf && node.enabled)
            {
                node.OnUpdate();
            }
        }


        // 更新该节点的叶子节点
        if (logicOnUpdateList != null)
        {
            ILogic logic = null;
            count = logicOnUpdateList.Count;
            for (i = 0; i < count; i++)
            {
                logic = logicOnUpdateList[i].logic;
                logic.OnUpdate();
            }
        }
    }
    public virtual void OnLateUpdate()
    {
        // 先更新该节点的子节点，在遍历了逻辑节点，深度优先遍历
        LogicNode node = null;
        int i = 0;
        int count = m_nodeList.Count;
        for (i = 0; i < count; i++)
        {
            node = m_nodeList[i];
            if (node.gameObject.activeSelf && node.enabled)
            {
                node.OnLateUpdate();
            }
        }


        // 更新该节点的叶子节点
        if (logicOnLateUpdateList != null)
        {
            ILogic logic = null;
            count = logicOnLateUpdateList.Count;
            for (i = 0; i < count; i++)
            {
                logic = logicOnLateUpdateList[i].logic;
                logic.OnLateUpdate();
            }
        }
    }
    #endregion

    private void OnApplicationQuit()
    {
        m_nodeList.Clear();
        m_logicNodeToUpdate.Clear();

        logicOnFixedUpdateList?.Clear();
        logicOnUpdateList?.Clear();
        logicOnLateUpdateList?.Clear();
        logciLeafDict.Clear();
        m_logicToUpdate.Clear();

        m_listenerList.Clear();
        m_listenerToUpdate.Clear();

        eventQueue_Next.Clear();
        eventQueue_Now.Clear();
    }
}

