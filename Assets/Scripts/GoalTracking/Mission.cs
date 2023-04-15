using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission 
{

    float goal;
    MissionType missionType;

    public enum MissionType
    {
        Distance, BananaCount, Trick, HazardCount, MaxSpeed, StyleCount
    };
    
    string description;
    public Mission(MissionType type, float goal)
    {
        switch (type)
        {
            case MissionType.Distance:
                description = "Achieve " + goal + " distance in one run.";
                break;
            case MissionType.BananaCount:
                description = "Collect " + goal + " bananas in one run.";
                break;
            case MissionType.Trick:
                description = "Perform " + goal + " tricks in one run.";
                break;
            case MissionType.HazardCount:
                description = "Collect " + goal + " distance in one run.";
                break;
            case MissionType.MaxSpeed:
                description = "Collect " + goal + " distance in one run.";
                break;
            case MissionType.StyleCount:
                description = "Collect " + goal + " distance in one run.";
                break;
            default:
                description = "";
                break;
        }
    }
}
