using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTrick : Trick
{
    Transform skateboard;

    public RightTrick(Transform skateboard)
    {
        this.skateboard = skateboard;
    }

    public override void EndTrick()
    {
        skateboard.localRotation = Quaternion.identity;
    }

    public override void StartTrick()
    {
        base.StartTrick();
        skateboard.Rotate(Vector3.forward, -60);
    }
}
