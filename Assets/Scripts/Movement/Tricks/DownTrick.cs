using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrick : Trick
{
    Transform skateboard;
    Transform gorilla;
    float boardRotationSpeed = 300f;
    Vector3 startingPosition;
    int announcerSFXID;

    public DownTrick(Transform skateboard, Transform player)
    {
        this.skateboard = skateboard;
        this.gorilla = player;
        announcerSFXID = SoundManager.Instance.GetSoundID("Announcer_Trick");
    }

    public override void DuringTrick()
    {
        skateboard.RotateAround(gorilla.position, Vector3.forward, boardRotationSpeed * Time.deltaTime);
    }

    public override void EndTrick()
    {
        skateboard.localRotation = Quaternion.identity;
        skateboard.localPosition = startingPosition;
        Goals_Tracker.instance?.trickTypeExecuted(GetType());
    }

    public override void StartTrick()
    {
        startingPosition = skateboard.localPosition;
        SoundManager.Instance.PlaySoundGlobal(announcerSFXID);
    }
}
