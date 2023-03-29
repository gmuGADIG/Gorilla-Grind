using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTrick : Trick
{
    Transform skateboard;

    public UpTrick(Transform skateboard)
    {
        this.skateboard = skateboard;
    }

    public override void StartTrick()
    {
        
    }

    public override void DuringTrick()
    {
        skateboard.Rotate(Vector3.forward, 300 * Time.deltaTime);
    }

    public override void EndTrick()
    {
        skateboard.localRotation = Quaternion.identity;
    }
}
