using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMission : Mission
{
    private static int[] randomGoals = { 10, 20, 30, 40, 50 };
    private PlayerMovement player;

    public HazardMission(PlayerMovement player) : this(player, randomGoals[Random.Range(0, randomGoals.Length)])
    { }

    public HazardMission(PlayerMovement player, int hazardGoal)
    {
        goal = hazardGoal;
        Name = "Hazard";
        Description = "Jump over " + goal + " hazards in one run";

        this.player = player;
        player.OnJumpedOverHazard.AddListener(IncrementProgress);
    }

    void IncrementProgress() => currentProgress += 1;

    public override void UpdateProgress()
    {
        // nothing
    }

    ~HazardMission()
    {
        player.OnJumpedOverHazard.RemoveListener(IncrementProgress);
    }
}
