using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
[CustomEditor(typeof(GroundSection))]
public class EditorGroundSection : Editor
{
    private readonly string SUBSECTION_PREFAB = "Assets/Prefabs/Subsection.prefab";

    //private SerializedProperty groundEdges;

    private void OnEnable()
    {
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.LabelField("The first subsection in the hierarchy will be used for generation (It is considered the main section)");
        if(IsOldModel() && GUILayout.Button("Migrate to subsection model"))
        {
            CreateSubsectionFromExisting();
            EditorUtility.DisplayDialog("Reminder", "Drag this prefab over the exisitng one in the assets folder to replace it. Choose to 'Create Base' when prompted to. Avoid undoing (control-z) until the prefab is saved or you may lose work.", "Close");
        }


        serializedObject.ApplyModifiedProperties();
    }

    bool IsOldModel()
    {
        return ((target as GroundSection).transform.childCount == 0 || (target as GroundSection).transform.GetChild(0).GetComponent<Subsection>() == null);
    }

    void CreateSubsectionFromExisting()
    {
        GroundSection gs = serializedObject.targetObject as GroundSection;
        GameObject subsectionPref = AssetDatabase.LoadAssetAtPath<GameObject>(SUBSECTION_PREFAB);
        GameObject subsection = Instantiate(subsectionPref,gs.transform);
        subsection.transform.SetAsFirstSibling();

        for (int i = 0; i < gs.transform.childCount; i++)
        {
            Transform child = gs.transform.GetChild(i);
            if(child.gameObject.GetComponent<GroundEdge>())
            {
                child.SetParent(subsection.transform);
                i--;//child count decrease when children are moved.
            }
        }

        
        
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawSpawnBounds(GroundSection gs, GizmoType gizmoType)
    {
        //Represents the bounds for when new sections get generated or despawned
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube((gs.endPoint + gs.startPoint)/2, (gs.endPoint-gs.startPoint) + Vector2.up * 50);
    }


}
