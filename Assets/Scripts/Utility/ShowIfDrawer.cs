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
            // �ʵ尡 Ư�� ���� ��ġ�ϴ��� Ȯ��
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

            // ���ǿ� ���� ���� �ʵ� ǥ��
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

            // ���ǿ� ���� ���� ���̸� ��ȯ
            if (show)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            else
            {
                return 0f; // ���ǿ� ���� ������ ���� 0
            }
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
