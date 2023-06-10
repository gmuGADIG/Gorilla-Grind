using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMission : Mission
{
    static int minGoal = 50;
    static int maxGoal = 200;

    public BananaMission() : this (Random.Range(minGoal, maxGoal))
    {
        RunController.Current.OnBananaCountChange += UpdateBananaCount;
    }

    public BananaMission(int bananaCount)
    {
        goal = bananaCount;
        Name = "Bananas";
        Description = "Get " + goal + " bananas in one run";
    }

    public override void UpdateProgress()
    {

    }

    void UpdateBananaCount(int count)
    {
        currentProgress = count;
    }

    ~BananaMission()
    {
        RunController.Current.OnBananaCountChange -= UpdateBananaCount;
    }
}
