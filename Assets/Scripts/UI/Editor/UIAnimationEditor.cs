using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIAnimation))]
public class UIAnimationEditor : Editor
{
    GameObject UIAnimation;
    private SerializedProperty animationType;
    private SerializedProperty animationMoveType;
    private void OnEnable()
    {

    }
    public override void OnInspectorGUI()
    {
        // 更新显示
        this.serializedObject.Update();
        //base.OnInspectorGUI();
        //运动类型
        animationMoveType = serializedObject.FindProperty("animationMoveType");

        EditorGUILayout.PropertyField(animationMoveType);
        //动画的类别
        animationType = serializedObject.FindProperty("animationType");

        EditorGUILayout.PropertyField(animationType);
        //参数调整
        if (animationType.enumValueIndex == 0 || animationType.enumValueIndex == 1 || animationType.enumValueIndex == 2 || animationType.enumValueIndex == 3 || animationType.enumValueIndex == 4)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("initialV3"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("targetV3"));
        }
        else if(animationType.enumValueIndex == 5)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("initialAlpha"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("targetAlpha"));
        }
        //激活时是否启动
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("enable"));
        ////延迟
        //var delay = serializedObject.FindProperty("delay");

        //EditorGUILayout.PropertyField(delay);

        //if (delay.boolValue == true) 
        //{
        //    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("delayTime"));
        //}

        // 分隔符
        EditorGUILayout.Separator();

        //动画曲线
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("animationCurveShow"));

        //EditorGUILayout.PropertyField(this.serializedObject.FindProperty("animationCurveHide"));


        serializedObject.ApplyModifiedProperties();
    }
}
