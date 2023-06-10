using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Banana, Distance, StylePoint, Hazard
}

public abstract class Mission
{
    protected float goal;
    protected float currentProgress;

    public float CurrentProgress => currentProgress;
    public float Goal => goal;
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    public static Mission GenerateRandomMission()
    {
        return null;
    }

    public virtual bool Complete()
    {
        if (currentProgress >= goal)
        {
            return true;
        }
        return false;
    }

    public abstract void UpdateProgress();
}
