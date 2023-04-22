using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemSpawnPoint))]
public class EditorItemSpawnPoint : Editor
{
    private void OnSceneGUI()
    {

        ItemSpawnPoint t = target as ItemSpawnPoint;
        //Debug.DrawLine(t.transform.position, t.PositionOnLine(t.transform.position));
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
