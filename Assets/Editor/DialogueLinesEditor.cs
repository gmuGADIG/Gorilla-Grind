using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DialogueLines))]
public class DialogueLinesEditor : Editor
{
    private int selectedEmotionIndex;

    public override void OnInspectorGUI()
    {
        DialogueLines dialogueLines = (DialogueLines)target;

        // Draw the default inspector GUI
        DrawDefaultInspector();

        for (int i = 0; i < dialogueLines.currentTalk.Length; i++)
        {
            if (dialogueLines.currentTalk[i].monkey != null)
            {
                List<Emotions> emotions = dialogueLines.currentTalk[i].monkey.emotion;
                List<string> emotionNames = new List<string>();

                foreach (Emotions emotion in emotions)
                {
                    emotionNames.Add(emotion.EmotionName);
                }

                int currentSelectedIndex = emotions.IndexOf(dialogueLines.currentTalk[i].emotion);
                int newSelectedIndex = EditorGUILayout.Popup("Emotion", currentSelectedIndex, emotionNames.ToArray());

                if (newSelectedIndex != currentSelectedIndex)
                {
                    dialogueLines.currentTalk[i].emotion = emotions[newSelectedIndex];
                }
            }
        }
    }
}
