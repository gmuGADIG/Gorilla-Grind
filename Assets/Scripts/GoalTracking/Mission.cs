using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission 
{

    float goal;
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
    public Mission(MissionType type, float goal, string trickType)
    {
        description = "Perform " + goal + " " + trickType + " Tricks in one run.";
        this.trickType = trickType;
    }

    public string getDescription()
    {
        return description;
    }

    public MissionType GetMissionType()
    {
        return missionType;
    }

    public float GetCurrentCount()
    {
        if (missionType == MissionType.Trick)
        {
            return tracker.GetCount(this.trickType);
        }
        else
        {
            return tracker.GetCount();
        }
    }

    public bool MetGoal()
    {
        return GetCurrentCount() >= goal;
    }

    public static Mission GetMissionFromDescription(string description)
    {
        string firstWord = description.Substring(0, description.IndexOf(" "));
        switch (firstWord)
        {
            case "Travel":
                return new Mission(MissionType.Distance, goalGenerator(MissionType.Distance));
                break;
            case "Collect":
                return new Mission(MissionType.BananaCount, goalGenerator(MissionType.BananaCount));
                break;
            case "Jump":
                return new Mission(MissionType.HazardCount, goalGenerator(MissionType.HazardCount));
                break;
            case "Achieve":
                return new Mission(MissionType.MaxSpeed, goalGenerator(MissionType.MaxSpeed));
                break;
            case "Obtain":
                return new Mission(MissionType.StyleCount, goalGenerator(MissionType.StyleCount));
                break;
            default:
                return new Mission(MissionType.Trick, goalGenerator(MissionType.Trick), trickRandomizer());
                break;
        }
    }

    static float goalGenerator(MissionType misType)
    {
        System.Random rnd = new System.Random();
        switch (misType)
        {
            case MissionType.Distance:
                return rnd.Next(1000, 10001);

            case MissionType.BananaCount:
                return rnd.Next(50, 101);

            case MissionType.MaxSpeed:
                return rnd.Next(50, 76);

            case MissionType.HazardCount:
                return rnd.Next(50, 101);

            case MissionType.Trick:
                return rnd.Next(4, 11);

            case MissionType.StyleCount:
                return rnd.Next(100, 1001);

            default:
                return 0;
        }
    }
    static string trickRandomizer()
    {
        System.Random rnd = new System.Random();
        string[] tricks = { "Up", "Down", "Left", "Right" };
        int num = rnd.Next(0, 4);
        return tricks[num];
    }
}
