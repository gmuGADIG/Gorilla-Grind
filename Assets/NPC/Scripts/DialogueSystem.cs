using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {
    public Text dialogueText;
    public Image headshotImage;
    public Text nameText;
    public DialogueLines dialogueLinesPrefab;
    public int currentDialogueIndex = 0;
    public float textSpeed = 0.1f; // The time delay between each letter appearing
    public AudioSource typingSound; // Add a public variable for the typing sound effect

    private DialogueLines.whosTalking currentCharacter;
    private string[] currentDialogueLines;
    private int currentLineIndex;
    private int currentCharacterLineIndex; // Counter for the current line index in the current character's dialogue lines
    private string currentLineText;
    private bool isTextAppearing = false;
    private bool isCurrentCharacterSpeaking = false; // Boolean to keep track of whether the current character is still speaking
    private int monkeySoundID;

    // Start is called before the first frame update
    void Start() {
        dialogueText.text = "";
        currentLineIndex = 0;
        currentCharacterLineIndex = 0;
        LoadDialogueLines();
        monkeySoundID = SoundManager.Instance.GetSoundID("Monkey_Noise");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Check if the current character is not speaking
            if (isCurrentCharacterSpeaking == true) {
                Debug.Log("The length of the current speaking guy is, " + currentDialogueLines.Length);
                Debug.Log("Index for current speaking guy is: " + currentCharacterLineIndex);
                // Check if there are more lines in the current character's dialogue
                if (currentCharacterLineIndex < currentDialogueLines.Length - 1)
                {
                    // If there are more lines, start the text animation coroutine with the next line of dialogue
                    currentCharacterLineIndex++;
                    StopAllCoroutines(); // Stop any previous text animation coroutine
                    Debug.Log(currentDialogueLines[currentCharacterLineIndex] + " and the index is " + currentCharacterLineIndex);
                    StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
                }
                else
                {
                    // If there are no more lines, move on to the next character's dialogue
                    currentDialogueIndex++;
                    if (currentDialogueIndex >= dialogueLinesPrefab.currentTalk.Length)
                    {
                        currentDialogueIndex = 0;
                    }
                    currentCharacterLineIndex = 0; // Reset currentCharacterLineIndex
                    LoadDialogueLines();
                }
            }
            // Check if the current line of the current talking monkey is being animated
            else if (isTextAppearing == true) {
                // If the current line is being animated, immediately display the rest of the text and stop the typing sound
                currentLineText += currentDialogueLines[currentCharacterLineIndex].Substring(currentLineText.Length);
                dialogueText.text = currentLineText;
                //typingSound.Stop();
                SoundManager.Instance.StopPlayingGlobal(monkeySoundID);
                isTextAppearing = false;
            }
            // If the current character is still speaking, move on to the next character's dialogue
            else {
                currentDialogueIndex++;
                if (currentDialogueIndex >= dialogueLinesPrefab.currentTalk.Length) {
                    currentDialogueIndex = 0;
                }
                LoadDialogueLines();
            }
        }
    }


    // Load the dialogue lines from the current DialogueLines prefab
    private void LoadDialogueLines() {
        currentCharacter = dialogueLinesPrefab.currentTalk[currentDialogueIndex];
        currentDialogueLines = currentCharacter.dialogueLines;
        currentLineIndex = 0;
        currentCharacterLineIndex = 0;

        // Update the UI elements for the current character
        headshotImage.sprite = currentCharacter.headshot;
        nameText.text = currentCharacter.monkeyName;
        Debug.Log("Headshot = " + headshotImage.sprite.name + " and the name = " + nameText.text);

        StopAllCoroutines(); // Stop any previous text animation coroutine
        StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
        isCurrentCharacterSpeaking = true; // Set the flag to indicate that the current character is still speaking
    }

    // Coroutine to animate the text to appear letter by letter
    private IEnumerator AnimateText(string line)
    {
        currentLineText = "";
        isTextAppearing = true;

        yield return new WaitForEndOfFrame();

        //typingSound.Play(); // enable the audio source and play the typing sound
        SoundManager.Instance.PlaySoundGlobal(monkeySoundID);
        for (int i = 0; i < line.Length; i++) {
            currentLineText += line[i];
            dialogueText.text = currentLineText;
            yield return new WaitForSeconds(textSpeed);
        }

        typingSound.Stop(); // stop the typing sound and disable the audio source
        isTextAppearing = false;

        // Check if the current character is still speaking
        if (currentCharacterLineIndex < currentDialogueLines.Length - 1) {
            // If the character is still speaking, increment the line index and set the flag to indicate that the current character is still speaking
            currentCharacterLineIndex++;
            isCurrentCharacterSpeaking = true;
        }
        else {
            // If the character has finished speaking, set the flag to indicate that the current character is no longer speaking
            isCurrentCharacterSpeaking = false;

            // Check if this is the last character's dialogue
            if (currentDialogueIndex == dialogueLinesPrefab.currentTalk.Length - 1) {
                // If it is, reset the dialogue index to the first character's dialogue
                currentDialogueIndex = 0;
            }
            else {
                // If it is not, move on to the next character's dialogue
                currentDialogueIndex++;
            }
            LoadDialogueLines();
            //typingSound.Stop(); // stop the typing sound and disable the audio source
            SoundManager.Instance.StopPlayingGlobal(monkeySoundID);
        }
    }
}
