using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonkeyMeetingManager : MonoBehaviour
{
    public static MonkeyMeetingManager Instance { get; private set; }

    public bool HasMeetingPending => nextMeeting != null;

    [SerializeField] MonkeyMeetingDialogue nextMeeting;

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
        SceneManager.activeSceneChanged += CheckForMonkeyMeetingScene;
        MonkeyMeeting.OnMonkeyMeetingEnd += CreateMonkeyMeetingMission;
    }

    private void CheckForMonkeyMeetingScene(Scene arg0, Scene arg1)
    {
        MonkeyMeeting meeting = FindObjectOfType<MonkeyMeeting>();
        if (meeting != null)
        {
            meeting.SetMeetingDialogue(nextMeeting);
        }
    }

    void CreateMonkeyMeetingMission()
    {
        if (nextMeeting.hasMission)
        {
            Mission mission = null;
            if (nextMeeting.linkedMissionType == MissionType.Banana)
            {
                mission = new BananaMission(nextMeeting.missionGoalCount);
            }
            else if (nextMeeting.linkedMissionType == MissionType.Distance)
            {
                mission = new DistanceMission(nextMeeting.missionGoalCount);
            }
            else if (nextMeeting.linkedMissionType == MissionType.Hazard)
            {
                mission = new HazardMission(nextMeeting.missionGoalCount);
            }
            else if (nextMeeting.linkedMissionType == MissionType.StylePoint)
            {
                mission = new StylePointMission(nextMeeting.missionGoalCount);
            }
            MissionManager.Instance.monkeyMeetingMission = mission;
        }
    }
}
