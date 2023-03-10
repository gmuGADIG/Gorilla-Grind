#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.ShortcutManagement;
using UnityEditor.EditorTools;
using System;
/**
* This class is used to provide gui features to make edge creation simpler.
*/
[CanEditMultipleObjects]
[CustomEditor(typeof(GroundEdge))]
public class EditorGroundEdge : Editor
{
    //private bool renderBounds;
    //private SerializedProperty renderBounds;
    private static EditorGroundEdge lastSelected;
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

        lastSelected = this;
    }
    

    private void OnSceneGUI()
    {
        //Im making code i don't fully know how to utilize
        //Handles.color = Color.red;
        //GroundEdge ground = target as GroundEdge;
        
        //Handles.DrawPolyLine(
        //    Utils.GetWorldPoints(
        //        Utils.Vec2ArrToVec3Arr(ground.edgeCollider.points), 
        //        ground.edgeCollider.gameObject
        //        )
        //    );
        return;
        //serializedObject.Update();
        //serializedObject.Update();
        //DrawBox(serializedObject.)
        //if(renderBounds.boolValue)
        //{
        //    Debug.DrawLine(b.min, b.max);
        //}
        //GroundEdge groundEdge = (GroundEdge)serializedObject.targetObject;
        //Handles.color = Color.red;
        //Handles.DrawLine(groundEdge.startPoint, groundEdge.endPoint);
    }


    //https://forum.unity.com/threads/creating-a-hotkey-to-get-into-edit-collider-mode.474706/#:~:text=I%20think%20you%20can%20hook%20into%20the%20Event,Mode%20%23_e%22%29%5D%20%2F%2F%20This%20is%20Shift%20%2B%20e
    //This feels very evil probably avoid copying too much
    [MenuItem("Tools/EnterEditMode %#e")]
    static void EnterEditMode()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (assembly.GetType("UnityEditor.EdgeCollider2DTool") != null)
            {
                ToolManager.SetActiveTool(assembly.GetType("UnityEditor.EdgeCollider2DTool"));
            }
        }
    }
}
#endif