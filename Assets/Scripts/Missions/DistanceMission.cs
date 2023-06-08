using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMission : Mission
{
    private static int[] randomGoals = { 100, 200, 300, 400, 500 };

    public DistanceMission() : this(randomGoals[Random.Range(0, randomGoals.Length)])
    { }

    public DistanceMission(float distanceGoal)
    {
        goal = distanceGoal;
        Name = "Distance:";
        Description = "Go " + distanceGoal + "m in one run.";
    }

    public override void UpdateProgress()
    {
        currentProgress += PlayerMovement.CurrentHorizontalSpeed * Time.deltaTime;
    }
}
