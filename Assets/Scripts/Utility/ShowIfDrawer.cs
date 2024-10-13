using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.conditionField);

        if (conditionProperty != null)
        {
            // 필드가 특정 값과 일치하는지 확인
            bool show = false;
            switch (conditionProperty.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    show = conditionProperty.boolValue.Equals(showIf.conditionValue);
                    break;
                case SerializedPropertyType.Enum:
                    show = conditionProperty.enumValueIndex.Equals((int)showIf.conditionValue);
                    break;
                case SerializedPropertyType.Integer:
                    show = conditionProperty.intValue.Equals(showIf.conditionValue);
                    break;
                case SerializedPropertyType.Float:
                    show = conditionProperty.floatValue.Equals((float)showIf.conditionValue);
                    break;
                case SerializedPropertyType.String:
                    show = conditionProperty.stringValue.Equals(showIf.conditionValue);
                    break;
            }

            // 조건에 맞을 때만 필드 표시
            if (show)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        else
        {
            Debug.LogWarning($"No matching property found for conditionField: {showIf.conditionField}");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.conditionField);

        if (conditionProperty != null)
        {
            bool show = false;
            switch (conditionProperty.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    show = conditionProperty.boolValue.Equals(showIf.conditionValue);
                    break;
                case SerializedPropertyType.Enum:
                    show = conditionProperty.enumValueIndex.Equals((int)showIf.conditionValue);
                    break;
                case SerializedPropertyType.Integer:
                    show = conditionProperty.intValue.Equals(showIf.conditionValue);
                    break;
                case SerializedPropertyType.Float:
                    show = conditionProperty.floatValue.Equals((float)showIf.conditionValue);
                    break;
                case SerializedPropertyType.String:
                    show = conditionProperty.stringValue.Equals(showIf.conditionValue);
                    break;
            }

            // 조건에 맞을 때만 높이를 반환
            if (show)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            else
            {
                return 0f; // 조건에 맞지 않으면 높이 0
            }
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
