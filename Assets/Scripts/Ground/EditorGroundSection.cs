using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
[CustomEditor(typeof(GroundSection))]
public class EditorGroundSection : Editor
{

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
