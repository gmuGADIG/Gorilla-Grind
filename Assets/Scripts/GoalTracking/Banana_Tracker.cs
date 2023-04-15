using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana_Tracker
{
    int bananaCount;
    int bananaGoal;

    public bool MetGoal()
    {
        return bananaCount >= bananaGoal;
    }

    public int GetBananas()
    {
        return bananaCount;
    }

    public void AddBananas(int count)
    {
        bananaCount += bananaGoal;
    }
}