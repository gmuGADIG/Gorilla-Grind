using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    [SerializeField] int numOfRandomMissions = 3;

    public int NumOfCurrentMissions => randomMissions.Count + (monkeyMeetingMission == null ? 0 : 1);
    public List<Mission> randomMissions = new List<Mission>();
    public Mission monkeyMeetingMission = null;

    int numOfMissionTypes = 4;

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
        DontDestroyOnLoad(gameObject);
        GenerateMissions();
    }

    public void GenerateMissions()
    {
        randomMissions.Clear();
        List<int> alreadyPickedMissionTypes = new List<int>(); // to prevent duplicate mission creation
        for (int i = 0; i <= numOfRandomMissions; i++)
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
                    newMission = new DistanceMission(100f);
                    break;
                case 1:
                    newMission = new TrickMission(10, typeof(UpTrick), FindObjectOfType<PlayerMovement>());
                    break;
                case 2:
                    newMission = new BananaMission(100);
                    break;
                case 3:
                    newMission = new HazardMission(20);
                    break;
            }
            randomMissions.Add(newMission);
        }
    }

    public void AddMission(Mission mission)
    {
        randomMissions.Add(mission);
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
        monkeyMeetingMission?.UpdateProgress();
    }
}
