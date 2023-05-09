using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MonkeyMeetingDialogue))]
public class MonkeyMeetingDialogueEditor : Editor
{
    private List<int> selectedEmotionIndices;

    public override void OnInspectorGUI()
    {
        MonkeyMeetingDialogue dialogueLines = (MonkeyMeetingDialogue)target;

        if (selectedEmotionIndices == null || selectedEmotionIndices.Count != dialogueLines.dialogue.Length)
        {
            selectedEmotionIndices = new List<int>(new int[dialogueLines.dialogue.Length]);
        }

        // Draw the default inspector GUI
        DrawDefaultInspector();

        for (int i = 0; i < dialogueLines.dialogue.Length; i++)
        {
            if (dialogueLines.dialogue[i].speakingCharacter != null)
            {
                List<Emotions> emotions = dialogueLines.dialogue[i].speakingCharacter.emotion;
                List<string> emotionNames = new List<string>();

                foreach (Emotions emotion in emotions)
                {
                    emotionNames.Add(emotion.EmotionName);
                }

                int currentSelectedIndex = emotions.IndexOf(dialogueLines.dialogue[i].emotion);
                int newSelectedIndex = EditorGUILayout.Popup("Emotion", currentSelectedIndex, emotionNames.ToArray());

                if (newSelectedIndex != currentSelectedIndex)
                {
                    dialogueLines.dialogue[i].emotion = emotions[newSelectedIndex];
                    selectedEmotionIndices[i] = newSelectedIndex;
                }
            }
        }
    }
}
