using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
[CustomEditor(typeof(GroundSection))]
public class EditorGroundSection : Editor
{
    //I referenced https://github.com/Unity-Technologies/UnityCsReference/blob/master/Modules/Physics2DEditor/Managed/Colliders/Collider2DEditorBase.cs
    private AnimBool m_ShowCompositeRedundants = new AnimBool();
    private SerializedProperty groundEdges;

    private void OnEnable()
    {
        groundEdges = serializedObject.FindProperty("groundEdges");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUI.BeginDisabledGroup(true);

        EditorGUILayout.PropertyField(groundEdges);

        EditorGUI.EndDisabledGroup();

        if(GUILayout.Button("Connect Edges"))
        {
            ((GroundSection)serializedObject.targetObject).VerifyConnections();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
