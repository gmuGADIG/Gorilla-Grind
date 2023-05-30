using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

[CreateAssetMenu(menuName = "Monkey Meetings/Dialogue")]
public class MonkeyMeetingDialogue : ScriptableObject
{
    //list of images of whos at the meeting

    public MonkeyMeetingCharacters characterDatabase;

    public DialogueFrame[] dialogueFrames;

    /* These are only here for the Dropdown menu attributes in DialogueFrame.
     * The [Dropdown] attribute only accepts the fully qualified name of a variable in string form.
     * These properties are the only way of generating them without using string literals. - Joe
     */
    List<CharacterData> Characters => characterDatabase.characters;
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
        public CharacterData speakingCharacter;
        [Dropdown(nameof(AllEmotions), nameof(name))]
        public Emotion emotion;
        [HideInInspector] public int myIndex;
        [HideInInspector] public List<Emotion> emotions;
        public string[] dialogueLines;
        public bool isMission;
        public bool isNarrator; // Add this line
        public bool isPlayerCharacter;
    }

    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
}
