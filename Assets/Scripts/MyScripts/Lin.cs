using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Lin
{
    /// <summary>
    /// 读取字典中的值，避免获取到null值。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool CheckDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, out TValue value)
    {
        if (!dict.TryGetValue(key, out value))
        {
            Debug.Log("配置表" + dict.Values.GetType() + "中找不到键值：" + key);
            return false;
        }
        return true;
    }
    /// <summary>
    /// 计算文本内容的总长度(像素点)
    /// </summary>
    /// <param name="message"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static int CalculateLengthOfText(string message, Text text)
    {
        int totalLength = 0;
        Font myFont = text.font;  //chatText is my Text component
        myFont.RequestCharactersInTexture(message, text.fontSize, text.fontStyle);
        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = message.ToCharArray();

        foreach (char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, text.fontSize);

            totalLength += characterInfo.advance;
        }

        return totalLength;
    }
    /// <summary>
    /// 设置物体子节点顺序
    /// </summary>
    /// <param name="obj">物体</param>
    /// <param name="index">设置成当前父节点的第index个位置</param>
    public static void SetGameObjectChildOrder(Transform obj,int index)
    {
        int childCount = obj.parent.childCount;

        if (index > childCount-1) { index = childCount -1; }

        obj.SetSiblingIndex(index);
    }
        /// <summary>
    /// 两点的角度
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static float PointToAngle(Vector2 p1, Vector2 p2)
    {
        Vector2 p;
        p.x = p2.x - p1.x;
        p.y = p2.y - p1.y;
        return Mathf.Atan2(p.y, p.x) * 180 / Mathf.PI;
    }
        /// <summary>
    /// 是否在屏幕里
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool IsInViewport(Transform target)
    {
        Transform aimTransfomr = target;

        Transform camreatra = Camera.main.transform;

        Vector3 viewPos = Camera.main.WorldToViewportPoint(aimTransfomr.position);

        Vector3 dir = (aimTransfomr.position - camreatra.position).normalized;

        float dot = Vector3.Dot(camreatra.forward, dir);
        //在屏幕里
        if (dot > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 获取v1,v2组成的平面的法线。v1 X V2 = V3 ==> V1 ⊥ V3 , V2 ⊥ V3;
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static Vector3 GetNormal(Vector3 v1,Vector3 v2)
    {
        return Vector3.Cross(v1,v2);
    }
    /// <summary>
    /// 获取当前节点下所有的子节点（不包含自身）
    /// </summary>
    /// <param name="list"></param>
    /// <param name="obj"></param>
    public static void GetAllChild(ref List<Transform> list, Transform obj,bool containSelf = false)
    {
        if (list.Count > 1000) { return; }
        if (containSelf) { list.Add(obj); }
        for (int i = 0; i < obj.childCount; ++i)
        {
            list.Add(obj.GetChild(i));
            GetAllChild(ref list, obj.GetChild(i),false);
        }
    }
        /// <summary>
    /// 洗牌
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    public static void ShuffleCards<T>(ref T[] array)
    {
        int count = array.Length;
        for (int i = 0; i < count; ++i)
        {
            int r = Random.Range(0, count);
            if (r != i)
            {
                T arrayT = array[i];
                array[i] = array[r];
                array[r] = arrayT;
            }
        }
    }
    /// <summary>
    /// 二次贝塞尔曲线
    /// </summary>
    public static Vector3 Bezier(Vector3 point0,Vector3 point1,Vector3 point2, float time)
    {
        time = Mathf.Clamp01(time);
        return Mathf.Pow(1 - time, 2) * point0 + 2 * time * (1 - time) * point1 + Mathf.Pow(time, 2) * point2;
    }
    /// <summary>
    /// 三次贝塞尔曲线
    /// </summary>
    public static Vector3 Bezier(Vector3 point0, Vector3 point1, Vector3 point2,Vector3 point3, float time)
    {
        time = Mathf.Clamp01(time);
        return Mathf.Pow(1 - time, 3) * point0 + 3 * time * Mathf.Pow(1 - time,2) * point1 + 3 * Mathf.Pow(time, 2) *(1-time) * point2 + Mathf.Pow(time,3) * point3;
    }
}

