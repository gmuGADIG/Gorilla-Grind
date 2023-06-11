using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonkeyMeetingManager : MonoBehaviour
{
    public static MonkeyMeetingManager Instance { get; private set; }

    public bool HasMeetingPending => currentMeeting != null;

    public MonkeyMeetingDialogue currentMeeting;

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

    void AtEndOfMeeting()
    {
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
            MissionManager.Instance.monkeyMeetingMission = mission;
        }
        currentMeeting = currentMeeting.nextMonkeyMeeting;
    }
}
