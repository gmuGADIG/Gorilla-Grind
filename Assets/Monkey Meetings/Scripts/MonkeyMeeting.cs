using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonkeyMeeting : MonoBehaviour
{
    public Text dialogueText;
    public Image headshotImage;
    public Image nameImage;
    public MonkeyMeetingDialogue meetingDialogue;
    public int currentDialogueIndex = 0;
    public float textSpeed = 0.1f;
    public AudioSource typingSound;
    public AudioSource monkeySoundSource;
    public int soundCharChange = 4;

    private MonkeyMeetingDialogue.DialogueFrame currentCharacter;
    private string[] currentDialogueLines;
    private int currentLineIndex;
    private int currentCharacterLineIndex;
    private string currentLineText;
    private bool isTextAppearing = false;
    private bool isCurrentCharacterSpeaking = false;
    public GameObject background;
    private bool dialogueStarted = false;


    void Start()
    {

    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
        background.SetActive(true); // Activate the background object
        dialogueText.text = "";
        currentLineIndex = 0;
        currentCharacterLineIndex = 0;
        currentDialogueIndex = 0;
        LoadDialogueLines();
        Debug.Log("Current character after loading dialogue lines: " + currentCharacter.speakingCharacter.name); // Add this line
        StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
        dialogueStarted = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueStarted)
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

                    if (currentDialogueIndex >= meetingDialogue.dialogueFrames.Length)
                    {
                        background.SetActive(false);
                        gameObject.SetActive(false);

                        for (int i = 0; i < meetingDialogue.dialogueFrames.Length; i++)
                        {
                            MonkeyMeetingDialogue.DialogueFrame character = meetingDialogue.dialogueFrames[i];
                            Emotions startingEmotion = GetSelectedEmotion(character);
                            if (startingEmotion != null)
                            {
                                Debug.Log(i);
                                Debug.Log(character.speakingCharacter.name);
                                Image image = transform.parent.Find(character.speakingCharacter.name).GetComponent<Image>();
                                image.enabled = false;

                                image.sprite = startingEmotion.characterSprite;
                            }
                        }

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

    private Emotions GetSelectedEmotion(MonkeyMeetingDialogue.DialogueFrame character)
    {
        if (character != null && character.speakingCharacter != null && character.myIndex >= 0 && character.myIndex < character.speakingCharacter.emotion.Count)
        {
            return character.speakingCharacter.emotion[character.myIndex];
        }
        else
        {
            return null;
        }
    }

    void LoadDialogueLines()
    {
        currentCharacter = meetingDialogue.dialogueFrames[currentDialogueIndex];
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
            MonkeyMeetingDialogue.DialogueFrame character = meetingDialogue.dialogueFrames[i];
            Emotions startingEmotion = GetSelectedEmotion(character);
            if (startingEmotion != null)
            {
                Debug.Log(i);
                Debug.Log(character.speakingCharacter.name);
                Image image = transform.parent.Find(character.speakingCharacter.name).GetComponent<Image>();
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
            Emotions emotion = GetSelectedEmotion(currentCharacter);
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
