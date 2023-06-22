using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMission : Mission
{
    private static int[] randomGoals = { 50, 100, 150, 200, 250 };

    public BananaMission() : this (randomGoals[Random.Range(0, randomGoals.Length)])
    {
    
    }


    public BananaMission(int bananaCount)
    {
        goal = bananaCount;
        Name = "Bananas";
        Description = "Get " + goal + " bananas in one run";
        missionType = MissionType.Banana;
        RunController.OnBananaCountChange += UpdateBananaCount;
    }

    public override void UpdateProgress()
    {

    }

    public override void UpdateProgress(float value)
    {
        currentProgress = value;
    }

    void UpdateBananaCount(int count)
    {
        currentProgress = count;
    }

    ~BananaMission()
    {
        RunController.OnBananaCountChange -= UpdateBananaCount;
    }
}
