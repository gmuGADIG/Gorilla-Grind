using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieToHazardMission : Mission
{
    public DieToHazardMission()
    {
        goal = 1;
        PlayerMovement.OnHitHazard += DeathByHazard;
        Name = "Die to Stinky Flower";
        Description = "End a run by hitting a stinky flower hazard";
    }

    public override void UpdateProgress()
    {

    }

    void DeathByHazard()
    {
        currentProgress = 1;
    }
}
