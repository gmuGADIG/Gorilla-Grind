using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterList", menuName = "Monkey Meetings/Characters")]
public class MonkeyMeetingCharacters : ScriptableObject
{
    public List<CharacterMaker> characters;

    public void Start()
    {
    }
}
