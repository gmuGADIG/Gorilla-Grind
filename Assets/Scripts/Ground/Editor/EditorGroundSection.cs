using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
[CustomEditor(typeof(GroundSection))]
public class EditorGroundSection : Editor
{
    private readonly string SUBSECTION_PREFAB = "Assets/Prefabs/Subsection.prefab";

    private SerializedProperty heightChangeProp;

    private void OnEnable()
    {
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        heightChangeProp = serializedObject.FindProperty("heightChange");
        EditorGUILayout.LabelField("The first subsection in the hierarchy will be used for generation (It is considered the main section)");
        if(IsOldModel() && GUILayout.Button("Migrate to subsection model"))
        {
            CreateSubsectionFromExisting();
            EditorUtility.DisplayDialog("Reminder", "Drag this prefab over the exisitng one in the assets folder to replace it. Choose to 'Create Base' when prompted to. Avoid undoing (control-z) until the prefab is saved or you may lose work.", "Close");
        }


        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.ExecuteCommand)
        {
            GroundSection section = target as GroundSection;
            foreach (Subsection subsection in section.subsections)
            {
                foreach (GroundEdge edge in subsection.groundEdges)
                {
                    edge.RenderLine();
                }
            }
        }
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
        int count = gs.transform.childCount;

        subsection.transform.SetAsFirstSibling();
        int uhhh = 0;//i thought this code shouldn't loop forever but it does sometimes so this variable is to break it.
        for (int i = 0; i < count; i++)
        {
            Transform child = gs.transform.GetChild(i);
            if(child.gameObject.GetComponent<GroundEdge>())
            {
                child.SetParent(subsection.transform);
                i--;//child count decrease when children are moved.
            }
            if (uhhh++ > count * 2) break;
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
