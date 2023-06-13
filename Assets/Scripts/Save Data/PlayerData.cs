using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class PlayerData
{

    [System.Serializable]
    public class MissionData
    {
        public MissionType type;
        public float goal;

        public MissionData(Mission m)
        {
            type = m.GetMissionType();
            goal = m.GetGoal();
        }

        public Mission LoadMission()
        {
            Mission m = null;
            switch (type)
            {
                case MissionType.Banana:
                    m = new BananaMission((int)goal);
                    break;
                case MissionType.Distance:
                    m = new DistanceMission(goal);
                    break;
                case MissionType.Hazard:
                    m = new HazardMission((int)goal);
                    break;
                case MissionType.Speed:
                    m = new SpeedMission(goal);
                    break;
                case MissionType.StylePoint:
                    m = new StylePointMission((int)goal);
                    break;
                default:    // in case all else fails somehow...
                    m = new BananaMission();
                    break;
            }
            

            return m;
        }
    }

    [System.Serializable]
    public class MeetingData
    {
        public int meetingNumber;

        public MeetingData(int num) => meetingNumber = num;

        public MonkeyMeetingDialogue LoadMeeting()
        {
            return MonkeyMeetingManager.Instance.allMeetings.meetings[meetingNumber];
        }
    }


    // inventory
    public int bananas;
    public string[] purchasedItems;
    public string equippedBoard;

    // mission manager
    // public MissionData[] randomMissions;     < removed because apparently missions are generated every run
    public MissionData storyMission;

    // meeting manager
    public MeetingData currentMeeting;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }

    public static PlayerData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerData>(jsonString);
    }

    public void SaveData(MissionManager missions, MonkeyMeetingManager meetings)
    {
        // inventory
        bananas = Inventory.getBananasInInventory();

        List<string> owned = Inventory.getOwnedBoards();
        purchasedItems = new string[owned.Count];
        for (int i = 0; i < owned.Count; i++)
        {
            purchasedItems[i] = owned[i];
        }

        equippedBoard = Inventory.getEquippedBoard();

        // missions
        // removed because missions are generated every run. but might be changed, who knows!
        /*
        List<Mission> missionObjects = missions.randomMissions;
        randomMissions = new MissionData[missionObjects.Count];
        for(int i = 0; i < missionObjects.Count; i++)
        {
            Mission m = missionObjects[i];
            randomMissions[i] = new MissionData(missionObjects[i]);
        }*/
        storyMission = new MissionData(missions.StoryMission);

        // meetings
        int meetIndex = 0;
        if (meetings.HasMeetingPending) meetIndex = meetings.currentMeeting.meetingIndex;
        currentMeeting = new MeetingData(meetIndex);
    }

    public void LoadData(MissionManager missions, MonkeyMeetingManager meetings)
    {
        // inventory
        Inventory.setBananas(bananas);
        foreach (string item in purchasedItems)
        {
            Inventory.addItem(String.Copy(item));
        }
        Inventory.equipBoard(String.Copy(equippedBoard));

        // mission manager
        /*
        foreach (MissionData mission in randomMissions)
        {
            missions.randomMissions.Add(mission.LoadMission());
        }
        */
        // ^ get with the program

        missions.SetStoryMission(storyMission.LoadMission());

        // meeting manager
        meetings.currentMeeting = currentMeeting.LoadMeeting();
    }

}

