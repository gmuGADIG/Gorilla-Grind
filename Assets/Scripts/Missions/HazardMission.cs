using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMission : Mission
{
    private static int[] randomGoals = { 2, 3, 4, 5, 6 };

    public HazardMission() : this(randomGoals[Random.Range(0, randomGoals.Length)])
    { }

    public HazardMission(int hazardGoal)
    {
        goal = hazardGoal;
        Name = "Hazard";
        Description = "Jump over " + goal + " hazards in one run";
        missionType = MissionType.Hazard;

        PlayerMovement.OnJumpedOverHazard += IncrementProgress;
    }

    void IncrementProgress() => currentProgress += 1;

    public override void UpdateProgress()
    {
        // nothing
    }

    public override void UpdateProgress(float value)
    {
        currentProgress = value;
    }

    ~HazardMission()
    {
        PlayerMovement.OnJumpedOverHazard -= IncrementProgress;
    }
}
