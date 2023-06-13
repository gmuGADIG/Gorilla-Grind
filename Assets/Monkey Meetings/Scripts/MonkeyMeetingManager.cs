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
        print("TEST" + gameObject.name);
        currentMeeting = meeting;
        if (currentMeeting.hasMission)
        {
            Mission mission = null;
            if (currentMeeting.linkedMissionType == MissionType.Banana)
            {
                mission = new BananaMission(currentMeeting.missionGoalCount);
            }
            else if (currentMeeting.linkedMissionType == MissionType.Distance)
            {
                mission = new DistanceMission(currentMeeting.missionGoalCount);
            }
            else if (currentMeeting.linkedMissionType == MissionType.Hazard)
            {
                mission = new HazardMission(currentMeeting.missionGoalCount);
            }
            else if (currentMeeting.linkedMissionType == MissionType.StylePoint)
            {
                mission = new StylePointMission(currentMeeting.missionGoalCount);
            }
            mission.unlockedMonkeyMeeting = currentMeeting.nextMonkeyMeeting;
            MissionManager.Instance.SetStoryMission(mission);
            currentMeeting = null;
        }
        else
        {
            currentMeeting = currentMeeting.nextMonkeyMeeting;
        }
    }

    private void OnDestroy()
    {
        MonkeyMeeting.OnMonkeyMeetingEnd -= AtEndOfMeeting;
    }
}
