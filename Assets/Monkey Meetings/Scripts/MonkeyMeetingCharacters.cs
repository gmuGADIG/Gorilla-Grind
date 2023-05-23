using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewCharacterList", menuName = "Monkey Meetings/Characters")]
public class MonkeyMeetingCharacters : ScriptableObject
{
    public List<CharacterData> characters;

    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
}
