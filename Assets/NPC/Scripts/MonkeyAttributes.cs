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
public class Emotions
{
    public string EmotionName;
    public Sprite emotion;
    public List<MonkeySounds> SoundMonkey;
    public string GetFirstSoundName()
    {
        if (SoundMonkey.Count > 0)
            return "Sound material: " + SoundMonkey[0].monkeyScream;
        else
            return "No Sound";
    }
    public MonkeySounds GetSoundAt(int index)
    {
        return SoundMonkey[index];
    }
}

[System.Serializable]
public class MonkeyData
{
    public string name;
    public Sprite NamePlate;
    public GameObject whereIam;
    public List<Emotions> emotion;
}

[System.Serializable]
public class MonkeyMaker : MonkeyData
{
}
