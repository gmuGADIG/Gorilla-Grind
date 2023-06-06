using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMission : Mission
{
    static int minGoal = 10;
    static int maxGoal = 50;

    public HazardMission() : this(Random.Range(minGoal, maxGoal))
    {

    }

    public HazardMission(int hazardGoal)
    {
        goal = hazardGoal;
        Name = "Hazard";
        Description = "Jump over " + goal + " hazards in one run";
    }

    public override void UpdateProgress()
    {

    }
}
