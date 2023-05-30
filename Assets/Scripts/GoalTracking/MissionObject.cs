using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject : ScriptableObject
{
    public static List<Mission> missions = new List<Mission>();
    static List<Mission.MissionType> missionTypes = new List<Mission.MissionType>() {
            Mission.MissionType.Distance,
            Mission.MissionType.BananaCount,
            Mission.MissionType.HazardCount,
            Mission.MissionType.MaxSpeed,
            Mission.MissionType.StyleCount,
            Mission.MissionType.Trick
        };
    public static void CreateNewMissions()
    {
        System.Random rnd = new System.Random();
        
        for (int i=0; i<3; i++)
        { 
            int num = rnd.Next(0, 6);
            switch (i)
            {
                case 1:
                    Mission.MissionType previous = missions[0].GetMissionType();
                    do
                    {
                        num = rnd.Next(0, 6);
                    } while (missionTypes[num] == previous);
                    break;
                case 2:
                    Mission.MissionType previous0 = missions[0].GetMissionType();
                    Mission.MissionType previous1 = missions[1].GetMissionType();
                    do
                    {
                        num = rnd.Next(0, 6);
                    } while (missionTypes[num] == previous0 || missionTypes[num] == previous1);
                    break;
                default:
                    break;


            }
            if (num < 5)
            {
                Debug.Log(num);
                Debug.Log(i);
                missions.Add(new Mission(missionTypes[num], goalGenerator(missionTypes[num])));
            }
            else
            {
               missions.Add(new Mission(missionTypes[num], goalGenerator(missionTypes[num]), trickRandomizer()));
            }
        }
    }

    static float goalGenerator(Mission.MissionType misType)
    {
        System.Random rnd = new System.Random();
        switch (misType)
        {
            case Mission.MissionType.Distance:
                return rnd.Next(1000, 10001);

            case Mission.MissionType.BananaCount:
                return rnd.Next(50, 101);

            case Mission.MissionType.MaxSpeed:
                return rnd.Next(50, 76);

            case Mission.MissionType.HazardCount:
                return rnd.Next(50, 101);

            case Mission.MissionType.Trick:
                return rnd.Next(4, 11);

            case Mission.MissionType.StyleCount:
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

    public static bool EvaluateMissions()
    {
        foreach(Mission mission in missions)
        {
            if (mission.MetGoal())
            {
                missions.Remove(mission);
            }
        }

        return missions.Count == 0;
    }

    public void AddMissionToListFromDescription(string description)
    {
        Mission monkeyMission = Mission.GetMissionFromDescription(description);
        missions.Add(monkeyMission);
    }

    public static List<Mission> GetCurrentMissions()
    {
        return missions;
    }
}
