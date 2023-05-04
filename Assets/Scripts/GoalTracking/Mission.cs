using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission 
{

    [HideInInspector] public float goal;
    MissionType missionType;
    Tracker tracker;
    string trickType;

    public enum MissionType
    {
        Distance, BananaCount, HazardCount, MaxSpeed, StyleCount, Trick
    };
    
    string description;
    public Mission(MissionType type, float goal)
    {
        this.goal = goal;
        missionType = type;
        switch (type)
        {
            case MissionType.Distance:
                description = "Travel a distance of " + goal + " in one run.";
                tracker = new Distance_Tracker();
                break;
            case MissionType.BananaCount:
                description = "Collect " + goal + " bananas in one run.";
                tracker = new Banana_Tracker();
                break;
            case MissionType.HazardCount:
                description = "Jump over " + goal + " hazards in one run.";
                tracker = new Hazards_Tracker();
                break;
            case MissionType.MaxSpeed:
                description = "Achieve a maximum speed of " + goal + " in one run.";
                tracker = new Speed_Tracker();
                break;
            case MissionType.StyleCount:
                description = "Obtain " + goal + " style points in one run.";
                tracker = new Style_Tracker();
                break;
            default:
                description = "";
                break;
        }
    }
    public Mission(MissionType type, float goal, string tType)
    {
        this.goal = goal;
        missionType = type;
        description = "Perform " + goal + " " + tType + " Tricks in one run.";
        tracker = new Trick_Tracker();
        this.trickType = tType;
    }

    public string getDescription()
    {
        return description;
    }

    public MissionType GetMissionType()
    {
        return missionType;
    }

    public float getCurrentProgress()
    {
        if (missionType == MissionType.Trick)
        {
            return tracker.GetCount(trickType);
        }
        else
        {
            return tracker.GetCount();
        }
    }

    public bool MetGoal()
    {
        if (missionType == MissionType.Trick)
        {
            return tracker.GetCount(trickType) >= goal;
        }
        else
        {
            return tracker.GetCount() >= goal;
        }
    }
}
