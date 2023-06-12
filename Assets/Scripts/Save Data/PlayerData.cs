using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    [System.Serializable]
    public class MissionData
    {
        public int type;
        public float goal;
        public float progress;

        public MissionData(Mission m)
        {
            type = m.GetMissionType();
            goal = m.GetGoal();
            progress = m.GetProgress();
        }
    }

    [System.Serializable]
    public class MeetingData
    {
        public int meetingNumber;

        public MeetingData(int num) => meetingNumber = num;
    }


    // inventory
    public int bananas;
    public string[] purchasedItems;
    public string equippedBoard;

    // mission manager
    public MissionData[] randomMissions;
    public MissionData storyMission;

    // meeting manager
    public MeetingData currentMeeting;

    public static string WriteToJson(PlayerData myData)
    {
        return JsonUtility.ToJson(myData);
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
        storyMission = new MissionData(missions.StoryMission);

        // meetings
        //currentMeeting = new MeetingData()
    }

}

