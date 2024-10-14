using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;  // 읽기 전용으로 설정
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;  // 다시 활성화
    }
}
