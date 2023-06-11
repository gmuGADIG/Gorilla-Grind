using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public int NumOfCurrentMissions => randomMissions.Count + (StoryMission == null ? 0 : 1);
    public List<Mission> randomMissions = new List<Mission>();
    public Mission StoryMission { get; private set; }

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
    }

    public void GenerateMissions()
    {
        randomMissions.Clear();
        List<int> alreadyPickedMissionTypes = new List<int>(); // to prevent duplicate mission creation
        for (int i = 0; i < numOfRandomMissions; i++)
        {
            Mission newMission = null;
            int missionType;
            do
            {
                missionType = Random.Range(0, numOfMissionTypes);
            }
            while (alreadyPickedMissionTypes.Contains(missionType));
            alreadyPickedMissionTypes.Add(missionType);
            switch (missionType)
            {
                case 0:
                    newMission = new DistanceMission();
                    break;
                case 1:
                    newMission = new StylePointMission();
                    break;
                case 2:
                    newMission = new BananaMission();
                    break;
                case 3:
                    newMission = new HazardMission();
                    break;
                case 4:
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
}
