using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewCharacterList", menuName = "Monkey Meetings/Character List")]
public class MonkeyMeetingCharacters : ScriptableObject
{
    public List<CharacterData> characters;
}
