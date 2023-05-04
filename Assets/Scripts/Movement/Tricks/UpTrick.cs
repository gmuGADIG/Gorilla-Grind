using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTrick : Trick
{
    Transform skateboard;
    int announcerSFXID;

    public UpTrick(Transform skateboard)
    {
        this.skateboard = skateboard;
        announcerSFXID = SoundManager.Instance.GetSoundID("Announcer_Trick");
    }

    public override void StartTrick()
    {
        
    }

    public override void DuringTrick()
    {
        skateboard.Rotate(Vector3.forward, 300 * Time.deltaTime);
        Goals_Tracker.instance?.trickTypeExecuted(GetType());
    }

    public override void EndTrick()
    {
        skateboard.localRotation = Quaternion.identity;
        SoundManager.Instance.PlaySoundGlobal(announcerSFXID);
    }
}
