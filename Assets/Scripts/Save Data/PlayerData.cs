using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerData
{

    [System.Serializable]
    public class MissionData
    {
        public MissionType type;
        public float goal;
        public int unlockedMeetingIndex;

        public MissionData(Mission m)
        {
            if (m != null)
            {
                type = m.GetMissionType();
                goal = m.GetGoal();
                if (m.unlockedMonkeyMeeting != null)
                    unlockedMeetingIndex = m.unlockedMonkeyMeeting.meetingIndex;
                else
                    unlockedMeetingIndex = 0;
            }
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
                case MissionType.DieToHazard:
                    m = new DieToHazardMission();
                    break;
            }

            if (m != null) m.unlockedMonkeyMeeting = MonkeyMeetingManager.Instance.allMeetings.meetings[unlockedMeetingIndex];
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

    public MissionData[] randomMissions = {};

    // meeting manager
    public MeetingData currentMeeting;

    public string ToJson()
    {
        if (storyMission == null)
        {
            Debug.Log("Story mission = null!");
        }
        var json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
        
        // ok so ToJson turns nulls into default values, so we have to check for such values and replace them with nulls
        // i hate it too
        if (storyMission.goal == 0) storyMission = null;
        if (currentMeeting.meetingNumber == 0) currentMeeting = null;
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
        List<Mission> missionObjects = missions.randomMissions;
        randomMissions = new MissionData[missionObjects.Count];
        for(int i = 0; i < missionObjects.Count; i++)
        {
            Mission m = missionObjects[i];
            randomMissions[i] = new MissionData(missionObjects[i]);
        }
        
        // story mission and monkey meeting
        if (missions.StoryMission != null)
        {
            storyMission = new MissionData(missions.StoryMission);
            currentMeeting = null;
        }
        else
        {
            storyMission = null;
            
            // set current meeting
            int meetIndex;
            if (missions.StoryMission != null && missions.storyMission.unlockedMonkeyMeeting != null)
                meetIndex = missions.StoryMission.unlockedMonkeyMeeting.meetingIndex;
            else if (meetings.currentMeeting != null) meetIndex = meetings.currentMeeting.meetingIndex;
            else meetIndex = 0;

            if (meetIndex == 0) currentMeeting = null;
            else currentMeeting = new MeetingData(meetIndex);
        }
    }

    public void LoadData(MissionManager missions, MonkeyMeetingManager meetings)
    {
        // inventory
        Inventory.setBananas(bananas);
        foreach (string item in purchasedItems)
        {
            Inventory.addItem(String.Copy(item));
        }
        Inventory.equipBoard(String.Copy(equippedBoard), false);

        // mission manager
        foreach (MissionData mission in this.randomMissions)
        {
            missions.randomMissions.Add(mission.LoadMission());
        }

        if (storyMission != null)
        {
            missions.SetStoryMission(storyMission.LoadMission());
            meetings.currentMeeting = null;
        }
        else
        {
            missions.SetStoryMission(null);
            meetings.currentMeeting = currentMeeting?.LoadMeeting();
        }
    }

}

