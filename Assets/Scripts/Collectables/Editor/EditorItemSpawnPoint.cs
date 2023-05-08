using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemSpawnPoint))]
[CanEditMultipleObjects]
public class EditorItemSpawnPoint : Editor
{

    private void OnSceneGUI()
    {

        ItemSpawnPoint t = target as ItemSpawnPoint;
        //Debug.DrawLine(t.transform.position, t.PositionOnLine(t.transform.position));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        EditorGUI.BeginChangeCheck();
        float orig = (target as ItemSpawnPoint).HeightOffset();
        float offset = EditorGUILayout.FloatField("Height Offset", orig);

        if(EditorGUI.EndChangeCheck())
        {
            ItemSpawnPoint item = target as ItemSpawnPoint;
            item.transform.position += (Vector3)Vector2.up * (offset - orig);

        }
        
    }

    [DrawGizmo(GizmoType.Selected, typeof(ItemSpawnPoint))]
    private static void DrawEdgeBox(ItemSpawnPoint itemSpawn, GizmoType type)
    {
        GroundEdge? edge = itemSpawn.groundEdge;
        if (!edge) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube((edge.startPoint + edge.endPoint) / 2, (edge.endPoint - edge.startPoint) + Vector2.up * 50);
    }
}
