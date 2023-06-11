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
    string[] boardNames;

    [SerializeField]
    GameObject[] boardPrefabs;

    public GameObject GetBoardPrefabFromName(string name)
    {
        int index = Array.FindIndex(boardNames, boardName => boardName == name);
        if (index != -1)
        {
            return boardPrefabs[index];
        }
        else
        {
            return null;
        }
    }
}
