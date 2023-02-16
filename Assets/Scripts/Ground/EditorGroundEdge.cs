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
        renderBounds.boolValue = EditorGUILayout.Toggle(renderBounds.boolValue, "Show bounds");
        //renderBounds = EditorGUILayout.Toggle(renderBounds, "Show bounds");


    }

    
    //private void OnSceneGUI()
    //{
    //    Bounds b = serializedObject.FindProperty("showBounds").boundsValue;
    //    DrawBox(serializedObject.)
    //}

    //private void DrawBox(Vector2 origin, Bounds b)
    //{
    //    Debug.DrawLine(origin, Vector2.up * b.size.y);
    //    Debug.DrawLine(origin, Vector2.right * b.size.x);
    //    //Debug.DrawLine(origin, Vector2.up * b.size.y);
    //    //Debug.DrawLine(origin, Vector2.up * b.size.y);
    //}
}
