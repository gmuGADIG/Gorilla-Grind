using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Subsection))]
public class EditorSubsection : Editor
{
    private SerializedProperty groundEdges;

    private void OnEnable()
    {
        groundEdges = serializedObject.FindProperty("groundEdges");
    }
    private void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.ExecuteCommand)
        {
            Subsection subsection = target as Subsection;
            foreach (GroundEdge edge in subsection.groundEdges)
            {
                edge.RenderLine();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        if (GUILayout.Button("Connect Edges"))
        {
            ((Subsection)serializedObject.targetObject).VerifyConnections();
        }
        GUILayout.Space(15);
        GUILayout.Label("The order here is set by the order in the Hierarchy");
        EditorGUI.BeginDisabledGroup(true);

        EditorGUILayout.PropertyField(groundEdges);

        EditorGUI.EndDisabledGroup();


        serializedObject.ApplyModifiedProperties();
    }
}
