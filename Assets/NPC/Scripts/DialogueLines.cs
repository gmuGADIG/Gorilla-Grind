using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueLines")]
public class DialogueLines : ScriptableObject
{
    //list of images of whos at the meeting
    public List<Sprite> whosHere;
    public List<string> whosWho;

    public whosTalking[] currentTalk;

    [System.Serializable]
    public class whosTalking {
        [Dropdown("whosHere")]
        public Sprite headshot;
        [Dropdown("whosWho")]
        public string monkeyName;
        public string[] dialogueLines;
    }

}
