using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTrick : Trick
{
    Transform skateboard;
    int announcerSFXID;

    public RightTrick(Transform skateboard)
    {
        this.skateboard = skateboard;
        announcerSFXID = SoundManager.Instance.GetSoundID("Announcer_Trick");
    }

    public override void DuringTrick()
    {

    }

    public override void EndTrick()
    {
        skateboard.localRotation = Quaternion.identity;
        Goals_Tracker.instance?.trickTypeExecuted(GetType());
    }

    public override void StartTrick()
    {
        skateboard.Rotate(Vector3.forward, -60);
        SoundManager.Instance.PlaySoundGlobal(announcerSFXID);
    }
}
