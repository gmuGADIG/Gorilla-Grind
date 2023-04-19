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
        //SceneView.duringSceneGui -= OnScene;
        //SceneView.duringSceneGui += OnScene;
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
        if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.ExecuteCommand)
        {
            (target as GroundEdge).SnapSurroundingEdges();
        }
        (target as GroundEdge).RenderLine();

        //Debug.Log(Event.current);
    }

    private void OnValidate()
    {
        GroundEdge t = target as GroundEdge;
        t.GetComponent<LineRenderer>().enabled = t.renderingLine;
        t.GetComponent<SpriteRenderer>().enabled = t.renderingSprite;
    }

    // T$$anonymous$$s function is called for each instance of "spawnPoint" in the scene. 
    // Make sure to pass the correct class in the first argument. In t$$anonymous$$s case ItemSpawnPoint
    // Make sure it is a "static" function
    // name it whatever you want
    //https://answers.unity.com/questions/654423/keep-my-custom-handle-visible-even-if-object-is-no.html
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandles(GroundEdge edge, GizmoType gizmoType)
    {
        if(edge.noCollision)
            RenderGroundEdge(edge);
    }

    private static void RenderGroundEdge(GroundEdge ground)
    {
        Handles.color = ground.noCollision ? GroundEdge.gapColor : GroundEdge.solidColor;
        //This is so that non-visible
        if (GroundEdge.shouldRenderEdge)
            Handles.DrawPolyLine(
                Utils.GetWorldPoints(
                    Utils.Vec2ArrToVec3Arr(ground.edgeCollider.points),
                    ground.edgeCollider.gameObject
                    )
                );
        
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

    //[MenuItem("Tools/Fix GroundEdge Rendering")]
      
}
#endif