using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Banana, Distance, StylePoint, Hazard, Speed
}

public abstract class Mission
{
    protected int missionType;
    protected float goal;
    protected float currentProgress;

    public float CurrentProgress => currentProgress;
    public float Goal => goal;
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public MonkeyMeetingDialogue unlockedMonkeyMeeting;

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

    public virtual int GetMissionType()
    {
        return missionType;
    }

    public virtual float GetGoal()
    {
        return goal;
    }

    public virtual float GetProgress()
    {
        return currentProgress;
    }

    public abstract void UpdateProgress();

}
