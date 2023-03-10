#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * This class is used to provide gui features to make edge creation simpler.
 */
[CanEditMultipleObjects]
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

    
}
#endif