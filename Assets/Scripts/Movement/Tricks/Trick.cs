using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class that all tricks inherit from.
/// </summary>
public abstract class Trick
{
    /// <summary>
    /// Called before the trick begins. Use to initialize/position skateboard/player.
    /// </summary>
    public abstract void StartTrick();
    /// <summary>
    /// Called every frame while the trick is being performed.
    /// </summary>
    public abstract void DuringTrick();
    /// <summary>
    /// Called when the trick ends. Use to reset player/skateboard positioning/rotation.
    /// </summary>
    public abstract void EndTrick();
}
