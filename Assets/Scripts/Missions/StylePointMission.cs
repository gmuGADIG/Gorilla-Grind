using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class StylePointMission : Mission
{
    private static int[] randomGoals = { 1000, 1500, 2000, 2500, 3000, 3500, 4000, 4500, 5000 };

    public StylePointMission() : this(randomGoals[Random.Range(0, randomGoals.Length)])
    { }

    public StylePointMission(int stylePointGoal)
    {
        goal = stylePointGoal;
        Name = "Style Points";
        Description = "Get " + goal + " style points in one run";
        RunController.OnStylePointChange += OnStylePointUpdate;
        missionType = MissionType.StylePoint;
    }

    private void OnStylePointUpdate(int totalPoints)
    {
        currentProgress = totalPoints;
    }

    public override void UpdateProgress()
    {
        // do nothing
    }

    public override void UpdateProgress(float value)
    {
        currentProgress = value;
    }

    ~StylePointMission()
    {
        RunController.OnStylePointChange -= OnStylePointUpdate;
    }
}
