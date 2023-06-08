using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TrickMission : Mission
{
    static List<Type> trickTypes = new List<Type>() { typeof(UpTrick), typeof(DownTrick), typeof(LeftTrick), typeof(RightTrick) };
    private static int[] randomGoals = { 10, 15, 20 };

    Type goalTrickType;
    PlayerMovement player;

    public TrickMission(PlayerMovement player) : this(randomGoals[Random.Range(0, randomGoals.Length)], trickTypes[Random.Range(0, trickTypes.Count)], player)
    { }

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
