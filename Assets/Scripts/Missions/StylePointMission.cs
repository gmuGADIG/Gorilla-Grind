using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class StylePointMission : Mission
{
    private static int[] randomGoals = { 10, 15, 20 };

    public StylePointMission() : this(randomGoals[Random.Range(0, randomGoals.Length)])
    { }

    public StylePointMission(int stylePointGoal)
    {
        goal = stylePointGoal;
        Name = "Style Points";
        Description = "Get " + goal + " style points in one run";
        RunController.OnStylePointChange += OnStylePointUpdate;
    }

    private void OnStylePointUpdate(int totalPoints)
    {
        currentProgress = totalPoints;
    }

    public override void UpdateProgress()
    {
        // do nothing
    }

    ~StylePointMission()
    {
        RunController.OnStylePointChange -= OnStylePointUpdate;
    }
}
