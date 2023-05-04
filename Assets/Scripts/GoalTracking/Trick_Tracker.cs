using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trick_Tracker : Tracker
{
    Type type;
    int goal;

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

    public override float GetCount()
    {
        return 0;
    }

    public override int GetCount(string type)
    {
        Debug.Log(type);
        return tricks[type];
    }
}
