using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueLines")]
public class DialogueLines : ScriptableObject
{
    public whosTalking[] currentTalk;

    [System.Serializable]
    public class whosTalking {
        public Sprite talkingMonkey;
        public string[] dialogueLines;
    }
}
