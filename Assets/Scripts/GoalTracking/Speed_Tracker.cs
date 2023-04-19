using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed_Tracker : Tracker
{
    float maxSpeed = 0;
    public override float GetCount()
    {
        return maxSpeed;
    }
    public override int GetCount(string type)
    {
        return 0;
    }

    public void SetMaxSpeed(float speed)
    {
        maxSpeed = speed;
    }
}
