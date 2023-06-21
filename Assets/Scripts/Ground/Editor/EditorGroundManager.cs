using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GroundManager))]
public class EditorGroundManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        if (GUILayout.Button(new GUIContent("Load Section Prefabs", "Loads the prefabs in the Assets/Prefabs/Ground Sections folder into the array")))
        {
            LoadPrefabs();
        }
    }

    [DrawGizmo(GizmoType.Selected)]
    static void DrawSpawnBounds(GroundManager gm, GizmoType gizmoType)
    {
        //Represents the bounds for when new sections get generated or despawned
        Gizmos.DrawWireCube(gm.transform.position, new Vector2(gm.spawnOffset, gm.spawnOffset) * 2);
    }

    void LoadPrefabs()
    {
        string[] sectionFolders = { "Assets/Prefabs/Ground Sections" };
        string[] sectionGUIDs = AssetDatabase.FindAssets("t:prefab", sectionFolders);
        Debug.Log(sectionGUIDs.Length + " sections fonud");
        
        GroundManager gm = target as GroundManager;
        serializedObject.Update();
        SerializedProperty sections = serializedObject.FindProperty("sectionPrefabs");
        sections.ClearArray();
        sections.arraySize = sectionGUIDs.Length;
        //gm.sectionPrefabs = new GameObject[sectionGUIDs.Length];
        for (int i = 0; i < sectionGUIDs.Length; i++)
        {
            string guid = sectionGUIDs[i];
            Debug.Log("Loading " + AssetDatabase.GUIDToAssetPath(guid));
            sections.GetArrayElementAtIndex(i).objectReferenceValue =  AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
        }
        serializedObject.ApplyModifiedProperties();
        Debug.Log("Done Loading");
    }
}
