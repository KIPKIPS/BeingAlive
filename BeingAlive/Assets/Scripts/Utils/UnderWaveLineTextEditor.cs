using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UnderWaveLineText)), CanEditMultipleObjects]
public class UnderWaveLineTextEditor : GraphicEditor {
    SerializedProperty m_Text;

    SerializedProperty m_FontData;

    SerializedProperty m_LangId;

    SerializedProperty m_FontKey;

    SerializedProperty m_UnderWaveAttr;

    protected override void OnEnable() {
        base.OnEnable();
        m_Text = this.serializedObject.FindProperty("m_Text");
        m_FontData = this.serializedObject.FindProperty("m_FontData");
        m_FontKey = this.serializedObject.FindProperty("fontKey");
        m_LangId = this.serializedObject.FindProperty("langId");
        m_UnderWaveAttr = this.serializedObject.FindProperty("m_UnderWaveLineAttr");
    }

    public override void OnInspectorGUI() {
        this.serializedObject.Update();
        if (m_FontKey != null) {
            EditorGUILayout.PropertyField(m_FontKey, new GUILayoutOption[0]);
        }
        if (m_LangId != null) {
            EditorGUILayout.PropertyField(m_LangId, new GUILayoutOption[0]);
        }
        EditorGUILayout.PropertyField(m_Text, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(m_FontData, new GUILayoutOption[0]);
        AppearanceControlsGUI();
        RaycastControlsGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("*下面是扩展内容*");
        EditorGUILayout.PropertyField(m_UnderWaveAttr, true);
        EditorGUILayout.HelpBox(@"下划波浪线的匹配正则式：<UnderWave/>内容</UnderWave>可选属性配置：<UnderWave/color=red,strength=0,length=0,thickness=1,offset=0>内容</UnderWave>", MessageType.Info);
        this.serializedObject.ApplyModifiedProperties();
    }
}
