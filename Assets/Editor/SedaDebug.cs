using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SedaDebug : MonoBehaviour
{
    [MenuItem("Seda/Safe Area iPhoneX")]
    static void SafeAreaForX()
	{
        Crystal.SafeArea.Sim = Crystal.SafeArea.SimDevice.iPhoneX;
    }
}
