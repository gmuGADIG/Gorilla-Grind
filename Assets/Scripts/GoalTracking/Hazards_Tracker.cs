using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards_Tracker : Tracker
{
    float count = 0;
    public override float GetCount()
    {
        return -1; // Not implemented;
    }
    public override int GetCount(string type)
    {
        return 0;
    }

    public void IncrementCount(int amount)
    {
        count += amount;
    }
}


