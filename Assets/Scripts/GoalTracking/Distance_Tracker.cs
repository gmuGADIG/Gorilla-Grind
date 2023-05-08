using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance_Tracker : Tracker
{
    float distanceCovered;
    float distanceGoal;
    
    public override float GetCount()
    {
        return distanceCovered;
    }

    public void AddDistance(float distance)
    {
        distanceCovered += distance;
    }
    public override int GetCount(string type)
    {
        return 0;
    }
}