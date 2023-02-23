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
    private SerializedProperty renderBounds;
    private void OnEnable()
    {
        renderBounds = serializedObject.FindProperty("showBounds");
        //renderBounds = false;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        renderBounds.boolValue = EditorGUILayout.Toggle("show bounds", renderBounds.boolValue);
        //renderBounds = EditorGUILayout.Toggle(renderBounds, "Show bounds");

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
        DrawBox(groundEdge.transform.position, groundEdge.boundSize);
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBox(Vector2 origin, Vector2 b)
    {
            
        Debug.DrawLine(origin - b / 2, origin +b.x*Vector2.right/2);
    }
}
