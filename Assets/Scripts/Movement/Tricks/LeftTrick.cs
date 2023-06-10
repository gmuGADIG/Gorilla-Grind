using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTrick : Trick
{
    Transform skateboard;
    int stylePointReward = 50;

    public LeftTrick(Transform skateboard)
    {
        this.skateboard = skateboard;
    }

    public override void DuringTrick()
    {

    }

    public override void EndTrick()
    {
        skateboard.localRotation = Quaternion.identity;
        RunController.Current.AddStylePoints(stylePointReward);
    }

    public override void StartTrick()
    {
        skateboard.Rotate(Vector3.forward, 60);
    }
}
