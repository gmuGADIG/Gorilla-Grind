using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrickMission : Mission
{
    Type goalTrickType;
    PlayerMovement player;

    public TrickMission(int trickGoalCount, Type goalTrickType, PlayerMovement player)
    {
        goal = trickGoalCount;
        this.goalTrickType = goalTrickType;
        this.player = player;
        this.player.OnTrickStart += CheckTrickType;
        Name = "Tricks";
        string trickName = "";
        if (goalTrickType == typeof(UpTrick))
        {
            trickName = "up trick";
        }
        else if (goalTrickType == typeof(DownTrick))
        {
            trickName = "down trick";
        }
        else if (goalTrickType == typeof(LeftTrick))
        {
            trickName = "left trick";
        }
        else if (goalTrickType == typeof(RightTrick))
        {
            trickName = "right trick";
        }
        Description = "Perform " + goal + " " + trickName + "s in one run";
    }

    private void CheckTrickType(Type trick)
    {
        if (trick == goalTrickType)
        {
            currentProgress++;
        }
    }

    public override void UpdateProgress()
    {
        // do nothing
    }

    ~TrickMission()
    {
        player.OnTrickStart -= CheckTrickType;
    }
}
