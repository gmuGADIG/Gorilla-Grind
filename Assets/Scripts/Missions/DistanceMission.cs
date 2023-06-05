using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMission : Mission
{

    public DistanceMission(float distanceGoal)
    {
        goal = distanceGoal;
        Name = "Distance:";
        Description = "Go " + distanceGoal + " without wiping out.";
    }

    public override void UpdateProgress()
    {
        currentProgress += PlayerMovement.CurrentHorizontalSpeed;
    }
}
