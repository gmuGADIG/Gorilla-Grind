using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonkeyMeetingManager : MonoBehaviour
{
    public static MonkeyMeetingManager Instance { get; private set; }

    public bool HasMeetingPending => currentMeeting != null;

    public MonkeyMeetingDialogue currentMeeting;

    public MonkeyMeetingsList allMeetings;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        if (transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        MonkeyMeeting.OnMonkeyMeetingEnd += AtEndOfMeeting;
    }

    void AtEndOfMeeting(MonkeyMeetingDialogue meeting)
    {
        currentMeeting = meeting;

        if (!currentMeeting.hasMission)
        {
            currentMeeting = currentMeeting.nextMonkeyMeeting;
        }
        else
        {
            currentMeeting = null;
        }
    }

    private void OnDestroy()
    {
        MonkeyMeeting.OnMonkeyMeetingEnd -= AtEndOfMeeting;
    }
}
