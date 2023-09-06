using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class that all tricks inherit from.
/// </summary>
public abstract class Trick
{
    protected int stylePointsPerSecond = 10;
    protected float addPointsInterval = .25f;
    protected float timer;

    /// <summary>
    /// Called before the trick begins. Use to initialize/position skateboard/player.
    /// </summary>
    public virtual void StartTrick()
    {
        timer = 0;
    }
    /// <summary>
    /// Called every frame while the trick is being performed.
    /// </summary>
    public virtual void DuringTrick()
    {
        timer += Time.deltaTime;
        if (timer >= addPointsInterval)
        {
            timer = 0;
            RunController.Current.AddStylePoints(stylePointsPerSecond);
        }
    }

    /// <summary>
    /// Called when the trick ends. Use to reset player/skateboard positioning/rotation.
    /// </summary>
    public abstract void EndTrick();
}
