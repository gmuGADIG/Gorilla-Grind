using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Monkey Meetings/Dialogue")]
public class MonkeyMeetingDialogue : ScriptableObject
{
    //list of images of whos at the meeting

    public MonkeyMeetingCharacters characterDatabase;

    public DialogueFrame[] dialogue = new DialogueFrame[0];

    List<CharacterMaker> Characters => characterDatabase.characters;

    [System.Serializable]
    public class DialogueFrame
    {
        [Dropdown(nameof(Characters), nameof(name))]
        public CharacterMaker speakingCharacter;
        [HideInInspector] public int myIndex;
        [HideInInspector] public List<Emotions> emotions;
        [HideInInspector] public Emotions emotion;
        public string[] dialogueLines;
        public bool isMission;
        public bool isNarrator; // Add this line
    }
}
