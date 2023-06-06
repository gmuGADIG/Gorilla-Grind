using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMission : Mission
{
    static int minDistance = 100;
    static int maxDistance = 500;

    public DistanceMission() : this(Random.Range(minDistance, maxDistance))
    {
        
    }

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
