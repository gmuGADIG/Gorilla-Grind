using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrick : Trick
{
    Transform skateboard;
    Transform gorilla;
    float boardRotationSpeed = 300f;
    Vector3 startingPosition;

    public DownTrick(Transform skateboard, Transform player)
    {
        this.skateboard = skateboard;
        this.gorilla = player;
    }

    public override void DuringTrick()
    {
        skateboard.RotateAround(gorilla.position, Vector3.forward, boardRotationSpeed * Time.deltaTime);
    }

    public override void EndTrick()
    {
        skateboard.localRotation = Quaternion.identity;
        skateboard.localPosition = startingPosition;
    }

    public override void StartTrick()
    {
        startingPosition = skateboard.localPosition;
    }
}
