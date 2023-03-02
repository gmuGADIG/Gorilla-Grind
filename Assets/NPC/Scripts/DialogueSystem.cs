using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public Text dialogueText;
    public DialogueLines dialogueLinesPrefab;
    public int currentDialogueIndex = 0;
    public float textSpeed = 0.1f; // The time delay between each letter appearing
    public AudioSource typingSound; // Add a public variable for the typing sound effect

    private string[] currentDialogueLines;
    private int currentLineIndex;
    private string currentLineText;
    private bool isTextAppearing = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = "";
        currentLineIndex = 0;
        LoadDialogueLines();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isTextAppearing == false)
        {
            currentLineIndex++;
            LoadDialogueLines();

            /*if (currentLineIndex >= currentDialogueLines.Length)
            {
                currentLineIndex = 0;

                currentDialogueIndex++;
                if (currentDialogueIndex >= dialogueLinesPrefab.Count)
                {
                    currentDialogueIndex = 0;
                }
                LoadDialogueLines();
            }*/

            StopAllCoroutines(); // Stop any previous text animation coroutine
            StartCoroutine(AnimateText(currentDialogueLines[currentLineIndex]));
        }
    }

    // Load the dialogue lines from the current DialogueLines prefab
    private void LoadDialogueLines()
    {
        currentDialogueLines = dialogueLinesPrefab.currentTalk[currentDialogueIndex].dialogueLines;
        Debug.Log(currentDialogueLines);
        currentLineIndex = 0;
        Debug.Log(currentDialogueLines[currentLineIndex]);

        StopAllCoroutines(); // Stop any previous text animation coroutine
        StartCoroutine(AnimateText(currentDialogueLines[currentLineIndex]));
    }

    // Coroutine to animate the text to appear letter by letter
    private IEnumerator AnimateText(string line)
    {
        currentLineText = "";
        isTextAppearing = true;

        yield return new WaitForEndOfFrame();

        typingSound.Play(); // enable the audio source and play the typing sound
        for (int i = 0; i < line.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isTextAppearing == true) 
            {
                dialogueText.text = currentDialogueLines[currentLineIndex];
                Debug.Log(dialogueText.text);
                break;
            }

            currentLineText += line[i];
            dialogueText.text = currentLineText;
            yield return new WaitForSeconds(textSpeed);

        }
        typingSound.Stop(); // stop the typing sound and disable the audio source

        isTextAppearing = false;
    }
}
