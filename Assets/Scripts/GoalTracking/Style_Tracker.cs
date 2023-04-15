using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Style_Tracker
{
    float stylePoints;
    float styleGoal;

    public bool MetGoal()
    {
        return stylePoints >= styleGoal;
    }

    public float GetStylePoints()
    {
        return stylePoints;
    }

    public void AddStylePoints(float points)
    {
        stylePoints += points;
    }
}
