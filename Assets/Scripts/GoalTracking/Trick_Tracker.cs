using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trick_Tracker
{
    Type type;
    int goal;
    int count;

    Dictionary<string, int> tricks = new Dictionary<string, int>() {
            {"Left", 0},
            {"Right", 0},
            {"Up", 0},
            {"Down", 0}
        };

    public void IncrementTrick(string type)
    {
        tricks[type] += 1;
    }

    public bool MetGoal()
    {
        return count >= goal;
    }

    public int GetCount()
    {
        return count;
    }
}
