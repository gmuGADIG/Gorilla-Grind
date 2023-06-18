using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public int NumOfCurrentMissions => randomMissions.Count + (StoryMission == null ? 0 : 1);
    public List<Mission> randomMissions = new List<Mission>();
    public Mission StoryMission { get => storyMission; private set => storyMission = value; }

    public Mission storyMission;

    bool nextMeetingUnlocked = false;
    int numOfRandomMissions = 3;
    int numOfMissionTypes = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        MonkeyMeeting.OnMonkeyMeetingEnd += GenerateMissions;
    }

    public void GenerateMissions(MonkeyMeetingDialogue dialogue)
    {
        randomMissions.Clear();
        List<MissionType> alreadyPickedMissionTypes = new List<MissionType>(); // to prevent duplicate mission creation
        for (int i = 0; i < numOfRandomMissions; i++)
        {
            Mission newMission = null;
            MissionType missionType;
            do
            {
                missionType = (MissionType) Random.Range(0, numOfMissionTypes);
            }
            while (alreadyPickedMissionTypes.Contains(missionType));
            alreadyPickedMissionTypes.Add(missionType);
            switch (missionType)
            {
                case MissionType.Distance:
                    newMission = new DistanceMission();
                    break;
                case MissionType.StylePoint:
                    newMission = new StylePointMission();
                    break;
                case MissionType.Banana:
                    newMission = new BananaMission();
                    break;
                case MissionType.Hazard:
                    newMission = new HazardMission();
                    break;
                case MissionType.Speed:
                    newMission = new SpeedMission();
                    break;
            }
            randomMissions.Add(newMission);
        }
    }

    public void SetStoryMission(Mission mission)
    {
        StoryMission = mission;
        nextMeetingUnlocked = false;
    }

    public void ResetAllMissionProgress()
    {
        if (StoryMission != null)
        {
            if (StoryMission.Complete())
            {
                StoryMission = null;
            }
            else
            {
                StoryMission.ResetProgress();
            }
        }
        for (int i = 0; i < randomMissions.Count; i++)
        {
            randomMissions[i].ResetProgress();
        }
    }

    private void Update()
    {
        for (int i = 0; i < randomMissions.Count; i++)
        {
            if (randomMissions[i] != null)
            {
                randomMissions[i].UpdateProgress();
            }
        }
        StoryMission?.UpdateProgress();
        // unlock the next monkey meeting when a story mission is complete.
        if (!nextMeetingUnlocked && StoryMission != null && StoryMission.Complete())
        {
            MonkeyMeetingManager.Instance.currentMeeting = StoryMission.unlockedMonkeyMeeting;
            nextMeetingUnlocked = true;
        }
    }

    private void OnDestroy()
    {
        MonkeyMeeting.OnMonkeyMeetingEnd -= GenerateMissions;
    }
}
