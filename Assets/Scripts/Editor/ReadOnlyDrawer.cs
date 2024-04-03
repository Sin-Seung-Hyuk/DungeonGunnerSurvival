using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 이전 GUI 스타일 저장
        var prevGUIState = GUI.enabled;

        // GUI 프로퍼티 비활성화
        GUI.enabled = false;

        // 프로퍼티 그리기
        EditorGUI.PropertyField(position, property, label);

        // GUI 이전세팅 가져오기
        GUI.enabled = prevGUIState;
    }
}
