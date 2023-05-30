using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewCharacterList", menuName = "Monkey Meetings/Characters")]
public class MonkeyMeetingCharacters : ScriptableObject
{
    public List<CharacterData> characters;

    //private void OnValidate()
    //{
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        characters[i].UpdateValues();
    //    }
    //}
}
