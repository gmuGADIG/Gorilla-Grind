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
        if (GUILayout.Button("Connect Edges"))
        {
            ((GroundSection)serializedObject.targetObject).VerifyConnections();
        }
        GUILayout.Space(15);
        GUILayout.Label("The order here is set by the order in the Hierarchy");
        EditorGUI.BeginDisabledGroup(true);

        EditorGUILayout.PropertyField(groundEdges);

        EditorGUI.EndDisabledGroup();

        
        serializedObject.ApplyModifiedProperties();
    }
}