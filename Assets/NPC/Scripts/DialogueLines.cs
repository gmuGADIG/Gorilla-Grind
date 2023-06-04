using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueLines")]
public class DialogueLines : ScriptableObject
{
    //list of images of whos at the meeting

    public MonkeyDataList database;

    public whosTalking[] currentTalk = new whosTalking[0];

    [System.Serializable]
    public class whosTalking
    {
        [Dropdown("database.Monkeys", "name")]
        public MonkeyMaker monkey;
        public int myIndex;
        public List<Emotions> emotions;
        public Emotions emotion;
        public string[] dialogueLines;
        public bool isNarrator; // Add this line
    }
}
