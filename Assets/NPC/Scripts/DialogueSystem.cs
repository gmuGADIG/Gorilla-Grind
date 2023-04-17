using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Text dialogueText;
    public Image headshotImage;
    public Text nameText;
    public DialogueLines dialogueLinesPrefab;
    public DialogueLines dialogueLines;
    public int currentDialogueIndex = 0;
    public float textSpeed = 0.1f;
    public AudioSource typingSound;
    public AudioSource monkeySoundSource;
    public int soundCharChange = 4;

    private DialogueLines.whosTalking currentCharacter;
    private string[] currentDialogueLines;
    private int currentLineIndex;
    private int currentCharacterLineIndex;
    private string currentLineText;
    private bool isTextAppearing = false;
    private bool isCurrentCharacterSpeaking = false;

    void Start()
    {

    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
        dialogueText.text = "";
        currentLineIndex = 0;
        currentCharacterLineIndex = 0;
        LoadDialogueLines();
        StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isTextAppearing)
            {
                if (currentCharacterLineIndex < currentDialogueLines.Length - 1)
                {
                    currentCharacterLineIndex++;
                    StopAllCoroutines();
                    StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
                    isCurrentCharacterSpeaking = true;
                }
                else
                {
                    currentDialogueIndex++;

                    if (currentDialogueIndex >= dialogueLinesPrefab.currentTalk.Length)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                    else
                    {
                        LoadDialogueLines();
                        StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
                    }
                }
            }
            else
            {
                StopAllCoroutines();
                currentLineText = currentDialogueLines[currentCharacterLineIndex];
                dialogueText.text = currentLineText;
                typingSound.Stop();
                monkeySoundSource.Stop();
                isTextAppearing = false;
            }
        }
    }

    private Emotions GetSelectedEmotion(DialogueLines.whosTalking character)
    {
        if (character != null && character.monkey != null && character.myIndex >= 0 && character.myIndex < character.monkey.emotion.Count)
        {
            return character.monkey.emotion[character.myIndex];
        }
        else
        {
            return null;
        }
    }

    void LoadDialogueLines()
    {
        currentCharacter = dialogueLinesPrefab.currentTalk[currentDialogueIndex];
        currentDialogueLines = currentCharacter.dialogueLines;
        currentCharacterLineIndex = 0;
        Emotions emotion = GetSelectedEmotion(currentCharacter);
        if (emotion != null)
        {
            nameText.text = currentCharacter.monkey.Name;
            headshotImage.sprite = emotion.emotion;
        }
    }

    private IEnumerator AnimateText(string line)
    {
        currentLineText = "";
        isTextAppearing = true;

        yield return new WaitForEndOfFrame();

        typingSound.Play();
        int characterCount = 0;
        Emotions emotion = GetSelectedEmotion(currentCharacter);

        for (int i = 0; i < line.Length; i++)
        {
            currentLineText += line[i];
            dialogueText.text = currentLineText;
            characterCount++;

            if (characterCount % soundCharChange == 0 && emotion != null)
            {
                monkeySoundSource.Stop(); // Stop any previous monkey sound
                PlayRandomMonkeySound(emotion); // Play a new random monkey sound
            }

            yield return new WaitForSeconds(textSpeed);
        }

        typingSound.Stop();
        monkeySoundSource.Stop();
        isTextAppearing = false;

        if (currentCharacterLineIndex < currentDialogueLines.Length - 1)
        {
            isCurrentCharacterSpeaking = true;
        }
        else
        {
            isCurrentCharacterSpeaking = false;
        }
    }

    private void PlayRandomMonkeySound(Emotions emotion)
    {
        if (emotion.SoundMonkey.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, emotion.SoundMonkey.Count);
            AudioClip randomSound = emotion.SoundMonkey[randomIndex].monkeyScream;
            monkeySoundSource.clip = randomSound;
            monkeySoundSource.Play();

            Debug.Log("Playing sound at index: " + randomIndex); // Add this line
        }
    }
}
