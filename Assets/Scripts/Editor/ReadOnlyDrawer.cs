using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // ���� GUI ��Ÿ�� ����
        var prevGUIState = GUI.enabled;

        // GUI ������Ƽ ��Ȱ��ȭ
        GUI.enabled = false;

        // ������Ƽ �׸���
        EditorGUI.PropertyField(position, property, label);

        // GUI �������� ��������
        GUI.enabled = prevGUIState;
    }
}
