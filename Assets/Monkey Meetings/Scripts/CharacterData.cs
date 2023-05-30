using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monkey Meetings/Character", fileName = "NewCharacter")]
public class CharacterData : ScriptableObject
{
    public Sprite NamePlate;
    public List<Emotion> emotions;
    public string characterSoundName;
}

//[System.Serializable]
//public class CharacterData : Character
//{
//}
