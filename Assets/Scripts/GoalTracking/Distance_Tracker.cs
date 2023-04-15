using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance_Tracker
{
    float distanceCovered;
    float distanceGoal;

    public bool MetGoal()
    {
        return distanceCovered >= distanceGoal;
    }

    public float GetDistanceCovered()
    {
        return distanceCovered;
    }

    public void AddDistance(float distance)
    {
        distanceCovered += distance;
    }
}