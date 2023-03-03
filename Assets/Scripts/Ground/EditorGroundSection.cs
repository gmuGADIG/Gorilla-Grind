using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GroundSection))]
public class EditorGroundSection : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        if(GUILayout.Button("Connect Edges"))
        {
            ((GroundSection)serializedObject.targetObject).VerifyConnections();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
