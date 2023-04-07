using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "MonkeyMeetingMakerObjects/MonkeyMaker")]
public class MonkeyMaker : ScriptableObject
{
    public string monkeyName;
    public MonkeyAttributes[] monkeyAttributes;

    [System.Serializable]
    public class MonkeyAttributes
    {
        public List<monkeyEmotions> emotions;
    }

    [System.Serializable]
    public class monkeyEmotions
    {
        public Sprite EmotionImage;
        public string EmotionName;
        public List<AudioSource> monkeySounds;
    }

}
