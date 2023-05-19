using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonkeyMeeting : MonoBehaviour
{
    public Text dialogueText;
    public Image headshotImage;
    public Image nameImage;
    public MonkeyMeetingDialogue meetingDialogue;
    public int currentDialogueFrameIndex = 0;
    public float textSpeed = 0.1f;
    public AudioSource typingSound;
    public AudioSource monkeySoundSource;
    public int soundCharChange = 4;

    private MonkeyMeetingDialogue.DialogueFrame currentCharacter;
    private string[] currentDialogueLines;
    private int currentCharacterLineIndex;
    private string currentLineText;
    private bool isTextCurrentlyAnimating = false;
    public GameObject background;
    private bool dialogueStarted = false;


    public void StartDialogue()
    {
        gameObject.SetActive(true);
        background.SetActive(true); // Activate the background object
        dialogueText.text = "";
        currentCharacterLineIndex = 0;
        currentDialogueFrameIndex = 0;
        LoadDialogueLines();
        Debug.Log("Current character after loading dialogue lines: " + currentCharacter.speakingCharacter.name); // Add this line
        StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
        dialogueStarted = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueStarted)
        {
            if (!isTextCurrentlyAnimating)
            {
                if (currentCharacterLineIndex < currentDialogueLines.Length - 1)
                {
                    PlayNextLine();
                }
                else
                {
                    currentDialogueFrameIndex++;

                    if (currentDialogueFrameIndex >= meetingDialogue.dialogueFrames.Length)
                    {
                        GoToNextDialogueFrame();
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
                isTextCurrentlyAnimating = false;
            }
        }
    }

    void PlayNextLine()
    {
        currentCharacterLineIndex++;
        StopAllCoroutines();
        StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
    }

    void GoToNextDialogueFrame()
    {
        background.SetActive(false);
        gameObject.SetActive(false);

        for (int i = 0; i < meetingDialogue.dialogueFrames.Length; i++)
        {
            MonkeyMeetingDialogue.DialogueFrame character = meetingDialogue.dialogueFrames[i];
            Emotion startingEmotion = GetSelectedEmotion(character);
            if (startingEmotion != null)
            {
                Debug.Log(i);
                Debug.Log(character.speakingCharacter.name);
                Image image = transform.parent.Find(character.speakingCharacter.name).GetComponent<Image>();
                image.enabled = false;

                image.sprite = startingEmotion.characterSprite;
            }
        }
    }

    private Emotion GetSelectedEmotion(MonkeyMeetingDialogue.DialogueFrame character)
    {
        if (character != null && character.speakingCharacter != null && character.myIndex >= 0 && character.myIndex < character.speakingCharacter.emotions.Count)
        {
            return character.speakingCharacter.emotions[character.myIndex];
        }
        else
        {
            return null;
        }
    }

    void LoadDialogueLines()
    {
        currentCharacter = meetingDialogue.dialogueFrames[currentDialogueFrameIndex];
        currentDialogueLines = currentCharacter.dialogueLines;
        currentCharacterLineIndex = 0;
        if (currentCharacter.isMission == true)
        {
            Debug.Log(currentDialogueLines[currentDialogueLines.Length - 1]);
            //AddMissionToListFromDescription(currentDialogueLines[currentDialogueLines.Length-1]);
        }

        // Set each monkey's GameObject to active and update their starting sprite
        for (int i = 0; i < meetingDialogue.dialogueFrames.Length; i++)
        {
            MonkeyMeetingDialogue.DialogueFrame dialogueFrame = meetingDialogue.dialogueFrames[i];
            Emotion startingEmotion = GetSelectedEmotion(dialogueFrame);
            if (startingEmotion != null)
            {
                Debug.Log(i);
                Debug.Log(dialogueFrame.speakingCharacter.name);
                Image image = transform.parent.Find(dialogueFrame.speakingCharacter.name).GetComponent<Image>();
                image.enabled = true;

                image.sprite = startingEmotion.characterSprite;
            }
        }

        if (currentCharacter.isNarrator)
        {
            nameImage.sprite = null;
        }
        else
        {
            Emotion emotion = GetSelectedEmotion(currentCharacter);
            if (emotion != null)
            {
                if (currentCharacter.speakingCharacter.NamePlate != null)
                {
                    nameImage.sprite = currentCharacter.speakingCharacter.NamePlate;
                    Debug.Log("Loading nameplate for: " + currentCharacter.speakingCharacter.name);
                    Debug.Log("Nameplate sprite: " + nameImage.sprite);
                }
                else
                {
                    Debug.LogError("Nameplate sprite is not assigned for monkey: " + currentCharacter.speakingCharacter.name);
                }
            }
        }
    }

    private IEnumerator AnimateText(string line)
    {
        currentLineText = "";
        isTextCurrentlyAnimating = true;

        yield return new WaitForEndOfFrame();

        typingSound.Play();
        int characterCount = 0;
        Emotion emotion = GetSelectedEmotion(currentCharacter);

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
        isTextCurrentlyAnimating = false;

    }

    private void PlayRandomMonkeySound(Emotion emotion)
    {
        if (emotion.emotionSound.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, emotion.emotionSound.Count);
            AudioClip randomSound = emotion.emotionSound[randomIndex].monkeyScream;
            monkeySoundSource.clip = randomSound;
            monkeySoundSource.Play();

            Debug.Log("Playing sound at index: " + randomIndex); // Add this line
        }
    }
}
