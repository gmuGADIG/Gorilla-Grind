using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MonkeyMeeting : MonoBehaviour
{
    public static event Action<MonkeyMeetingDialogue> OnMonkeyMeetingEnd;

    public TMP_Text dialogueText;
    public Image nameImage;
    public Transform charactersParentObject;
    public Transform highlightImage;
    [SerializeField] MonkeyMeetingDialogue meetingDialogue;
    public float textSpeed = 0.033f;
    public int soundCharChange = 4;
    [SerializeField] string postRunSceneName;

    private MonkeyMeetingDialogue.DialogueFrame dialogueFrame;
    private string[] currentDialogueLines;
    private int currentDialogueFrameIndex = 0;
    private int currentCharacterLineIndex;
    private string currentLineText;
    private bool isTextCurrentlyAnimating = false;
    private bool dialogueStarted = false;

    private void Start()
    {
        if (MonkeyMeetingManager.Instance != null)
        {
            meetingDialogue = MonkeyMeetingManager.Instance.currentMeeting;
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        //gameObject.SetActive(true);
        //background.SetActive(true); // Activate the background object
        // lock/unlock boards
        if (meetingDialogue.unlockedBoard != "")
        {
            Inventory.UnlockItem(meetingDialogue.unlockedBoard);
        }
        if (meetingDialogue.lockedBoard != "")
        {
            Inventory.LockItem(meetingDialogue.lockedBoard);
        }
        print(meetingDialogue.name);
        dialogueText.text = "";
        currentCharacterLineIndex = 0;
        currentDialogueFrameIndex = 0;
        SetupCharacters();
        LoadNextDialogueFrame();
        Debug.Log("Current character after loading dialogue lines: " + dialogueFrame.speakingCharacter.name); // Add this line
        StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
        dialogueStarted = true;
    }

    /// <summary>
    /// Activates the sprite gameobjects for all characters who are in this monkey meeting
    /// </summary>
    void SetupCharacters()
    {
        // get list of all characters in meeting
        List<string> characterNames = new List<string>();
        for (int i = 0; i < meetingDialogue.dialogueFrames.Length; i++)
        {
            if (meetingDialogue.dialogueFrames[i].speakingCharacter == null) print("TEST");
            if (!characterNames.Contains(meetingDialogue.dialogueFrames[i].speakingCharacter.name))
            {
                characterNames.Add(meetingDialogue.dialogueFrames[i].speakingCharacter.name);
            }
        }
        // set those characters' sprites active
        for (int i = 0; i < characterNames.Count; i++)
        {
            if (characterNames[i] != "Narrator")
            {
                Transform character = GetSceneCharacter(characterNames[i]).GetChild(0);
                if (character != null)
                {
                    character.gameObject.SetActive(true);
                }
            }
        }
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
                        EndDialogue();
                        return;
                    }
                    else
                    {
                        LoadNextDialogueFrame();
                        StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
                    }
                }
            }
            else
            {
                StopAllCoroutines();
                currentLineText = currentDialogueLines[currentCharacterLineIndex];
                dialogueText.text = currentLineText;
                isTextCurrentlyAnimating = false;
            }
        }
        if (dialogueStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialogue();
        }
    }

    void PlayNextLine()
    {
        currentCharacterLineIndex++;
        StopAllCoroutines();
        StartCoroutine(AnimateText(currentDialogueLines[currentCharacterLineIndex]));
    }

    void EndDialogue()
    {
        dialogueStarted = false;
        OnMonkeyMeetingEnd?.Invoke(meetingDialogue);
        meetingDialogue = null;
        SceneManager.LoadScene(postRunSceneName);
    }

    private Emotion GetSelectedEmotion(MonkeyMeetingDialogue.DialogueFrame frame)
    {
        if (frame != null && frame.speakingCharacter != null && frame.myIndex >= 0 && frame.myIndex < frame.speakingCharacter.emotions.Count)
        {
            return frame.speakingCharacter.emotions[frame.myIndex];
        }
        else
        {
            return null;
        }
    }

    void LoadNextDialogueFrame()
    {
        dialogueFrame = meetingDialogue.dialogueFrames[currentDialogueFrameIndex];
        currentDialogueLines = dialogueFrame.dialogueLines;
        currentCharacterLineIndex = 0;

        if (dialogueFrame.isNarrator || dialogueFrame.isPlayerCharacter)
        {
            nameImage.gameObject.SetActive(false);
            dialogueText.fontStyle = FontStyles.Italic;
        }
        else
        {
            nameImage.gameObject.SetActive(true);
            dialogueText.fontStyle = FontStyles.Normal;
            // set speaking character's emotion
            Image characterSprite = GetSceneCharacter(dialogueFrame.speakingCharacter.name).GetChild(0).GetComponent<Image>();
            if (characterSprite != null)
            {
                characterSprite.sprite = dialogueFrame.emotion.sprite;
            }
            SetCharacterHighlight(dialogueFrame);

            Emotion emotion = GetSelectedEmotion(dialogueFrame);
            if (emotion != null)
            {
                print(meetingDialogue.dialogueFrames[currentDialogueFrameIndex].speakingCharacter.NamePlate);
                if (dialogueFrame.speakingCharacter.NamePlate != null)
                {
                    nameImage.sprite = dialogueFrame.speakingCharacter.NamePlate;
                    Debug.Log("Loading nameplate for: " + dialogueFrame.speakingCharacter.name);
                    Debug.Log("Nameplate sprite: " + nameImage.sprite);
                }
                else
                {
                    Debug.LogError("Nameplate sprite is not assigned for monkey: " + dialogueFrame.speakingCharacter.name);
                }
            }
        }
    }

    private IEnumerator AnimateText(string line)
    {
        currentLineText = "";
        isTextCurrentlyAnimating = true;
        int characterSoundID = SoundManager.Instance.GetSoundID(dialogueFrame.speakingCharacter.characterSoundName);

        yield return new WaitForEndOfFrame();

        //typingSound.Play();
        int characterCount = 0;
        Emotion emotion = GetSelectedEmotion(dialogueFrame);

        for (int i = 0; i < line.Length; i++)
        {
            currentLineText += line[i];
            dialogueText.text = currentLineText;
            characterCount++;

            // play character sound
            if (!dialogueFrame.isNarrator && !dialogueFrame.isPlayerCharacter && characterCount % soundCharChange == 0 && emotion != null)
            {
                SoundManager.Instance.PlaySoundGlobal(characterSoundID);
            }

            yield return new WaitForSeconds(textSpeed);
        }
        isTextCurrentlyAnimating = false;

    }

    Transform GetSceneCharacter(string name)
    {
        return charactersParentObject.Find(name);
    }

    void SetCharacterHighlight(MonkeyMeetingDialogue.DialogueFrame dialogueFrame)
    {
        highlightImage.gameObject.SetActive(true);
        highlightImage.SetSiblingIndex(charactersParentObject.childCount - 1);
        Transform highlightedCharacter = GetSceneCharacter(dialogueFrame.speakingCharacter.name);
        highlightedCharacter.SetSiblingIndex(charactersParentObject.childCount - 1);
    }
}
