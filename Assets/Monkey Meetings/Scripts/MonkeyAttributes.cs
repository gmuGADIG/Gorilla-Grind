using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public Sprite characterSprite;
    public List<MonkeySounds> emotionSound;
    public string GetFirstSoundName()
    {
        if (emotionSound.Count > 0)
            return "Sound material: " + emotionSound[0].monkeyScream;
        else
            return "No Sound";
    }
    public MonkeySounds GetSoundAt(int index)
    {
        return emotionSound[index];
    }
}

[System.Serializable]
public class CharacterData
{
    public string name;
    public Sprite NamePlate;
    public GameObject characterGameObject;
    public List<Emotion> emotions;
}

[System.Serializable]
public class CharacterMaker : CharacterData
{
}
