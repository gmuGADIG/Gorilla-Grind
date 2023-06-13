using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewDialogueList", menuName = "Monkey Meetings/Dialogue List")]
public class MonkeyMeetingsList : ScriptableObject
{
    public List<MonkeyMeetingDialogue> meetings;
}