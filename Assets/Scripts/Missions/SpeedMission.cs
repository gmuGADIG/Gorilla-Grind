using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMission : Mission
{
    private static int[] randomGoals = { 35, 40, 45 };
    private float maxSpeed = 0;

    public SpeedMission() : this(randomGoals[Random.Range(0, randomGoals.Length)])
    { }

    public SpeedMission(float speedGoal)
    {
        goal = speedGoal;
        Name = "Speed:";
        Description = "Achieve a speed of " + speedGoal + "m/s.";
        missionType = MissionType.Speed;
    }

    public override void UpdateProgress()
    {
        maxSpeed = Mathf.Max(maxSpeed, PlayerMovement.CurrentHorizontalSpeed);
        currentProgress = maxSpeed;
    }

    public override void UpdateProgress(float value)
    {
        currentProgress = value;
    }
}