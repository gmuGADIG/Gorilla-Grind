using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana_Tracker : Tracker
{
    int bananaCount;
    int bananaGoal;

    public override float GetCount()
    {
        return bananaCount;
    }

    public void AddBananas(int count)
    {
        bananaCount += bananaGoal;
    }

    public override int GetCount(string type)
    {
        return 0;
    }
}