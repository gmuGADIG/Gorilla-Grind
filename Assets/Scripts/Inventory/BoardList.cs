using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// List of the boards available in the game. Mapped string names -> prefabs.
/// </summary>
[CreateAssetMenu(fileName = "NewBoardList", menuName = "Board List")]
public class BoardList : ScriptableObject
{
    [SerializeField]
    BoardEntry[] boards;

    [System.Serializable]
    class BoardEntry
    {
        public string name;
        public GameObject prefab;
    }

    public GameObject GetBoardPrefabFromName(string name)
    {
        for (int i = 0; i < boards.Length; i++)
        {
            if (boards[i].name == name)
            {
                return boards[i].prefab;
            }
        }
        Debug.LogError("Board With Name:" + name + "Not Found in Board List");
        return null;
    }
}
