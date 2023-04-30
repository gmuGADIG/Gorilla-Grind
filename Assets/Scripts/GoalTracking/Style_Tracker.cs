using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Style_Tracker : Tracker
{
    float stylePoints;
    float styleGoal;

    public override float GetCount()
    {
        return stylePoints;
    }

    public void AddStylePoints(float points)
    {
        stylePoints += points;
    }

    public override int GetCount(string type)
    {
        return 0;
    }
}
