using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMission : Mission
{
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
