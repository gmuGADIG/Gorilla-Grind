using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * This class is used to provide gui features to make edge creation simpler.
 */
[CustomEditor(typeof(GroundEdge))]
public class EditorGroundEdge : Editor
{
    //private bool renderBounds;
    //private SerializedProperty renderBounds;

    private void OnEnable()
    {
        //renderBounds = serializedObject.FindProperty("showBounds");
        //renderBounds = false;
    }
   
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        //renderBounds.boolValue = EditorGUILayout.Toggle("show bounds", renderBounds.boolValue);
        //renderBounds = EditorGUILayout.Toggle(renderBounds, "Show bounds");
        //Nothing to do here currently. Can change later.
        GroundEdge.shouldRenderEdge = EditorGUILayout.Toggle("Show Edge", GroundEdge.shouldRenderEdge);
        serializedObject.ApplyModifiedProperties();
    }


    private void OnSceneGUI()
    {
        serializedObject.Update();
        //serializedObject.Update();
        //DrawBox(serializedObject.)
        //if(renderBounds.boolValue)
        //{
        //    Debug.DrawLine(b.min, b.max);
        //}
        GroundEdge groundEdge = (GroundEdge)serializedObject.targetObject;
        //Handles.color = Color.red;
        //Handles.DrawLine(groundEdge.startPoint, groundEdge.endPoint);
        serializedObject.ApplyModifiedProperties();
    }

}
