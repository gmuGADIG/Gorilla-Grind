using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class MonkeySounds
{
    public AudioClip monkeyScream;
    public string GetMaterialNameWithPrefix(string p)
    {
        return p + monkeyScream;
    }
}

[System.Serializable]
public class Emotion
{
    public string name;
    public Sprite sprite;
}

//[System.Serializable]
//public class CharacterData : Character
//{
//}
