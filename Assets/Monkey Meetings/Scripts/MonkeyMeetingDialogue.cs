using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(menuName = "Monkey Meetings/Dialogue")]
public class MonkeyMeetingDialogue : ScriptableObject
{
    //list of images of whos at the meeting

    public MonkeyMeetingCharacters characterDatabase;

    public DialogueFrame[] dialogueFrames = new DialogueFrame[0];

    List<CharacterMaker> Characters => characterDatabase.characters;


    List<Emotion> AllEmotions => GetAllEmotions();
    List<Emotion> GetAllEmotions()
    {
        List<Emotion> allEmotions = new List<Emotion>();
        for (int i = 0; i < characterDatabase.characters.Count; i++)
        {
            allEmotions.AddRange(characterDatabase.characters[i].emotions);
        }
        return allEmotions;
    }


    [System.Serializable]
    public class DialogueFrame
    {
        List<Emotion> CharacterEmotions => speakingCharacter.emotions;

        [Dropdown(nameof(Characters), nameof(name))]
        public CharacterMaker speakingCharacter;
        [Dropdown(nameof(AllEmotions), nameof(name))]
        public Emotion emotion;
        [HideInInspector] public int myIndex;
        [HideInInspector] public List<Emotion> emotions;
        //[HideInInspector] public Emotion emotion;
        public string[] dialogueLines;
        public bool isMission;
        public bool isNarrator; // Add this line
    }
}
