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

        if (selectedEmotionIndices == null || selectedEmotionIndices.Count != dialogueLines.dialogueFrames.Length)
        {
            selectedEmotionIndices = new List<int>(new int[dialogueLines.dialogueFrames.Length]);
        }

        // Draw the default inspector GUI
        DrawDefaultInspector();

        for (int i = 0; i < dialogueLines.dialogueFrames.Length; i++)
        {
            if (dialogueLines.dialogueFrames[i].speakingCharacter != null)
            {
                List<Emotions> emotions = dialogueLines.dialogueFrames[i].speakingCharacter.emotion;
                List<string> emotionNames = new List<string>();

                foreach (Emotions emotion in emotions)
                {
                    emotionNames.Add(emotion.EmotionName);
                }

                int currentSelectedIndex = emotions.IndexOf(dialogueLines.dialogueFrames[i].emotion);
                int newSelectedIndex = EditorGUILayout.Popup("Emotion", currentSelectedIndex, emotionNames.ToArray());

                if (newSelectedIndex != currentSelectedIndex)
                {
                    dialogueLines.dialogueFrames[i].emotion = emotions[newSelectedIndex];
                    selectedEmotionIndices[i] = newSelectedIndex;
                }
            }
        }
    }
}
