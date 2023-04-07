using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleBoard : Skateboard
{
    [SerializeField] float rotationPercentageIncrease = .5f;

    public override void ApplyBonuses(PlayerMovement movement)
    {
        movement.RotationMultiplier = 1 + rotationPercentageIncrease;
    }

    public override void RemoveBonuses(PlayerMovement movement)
    {
        movement.RotationMultiplier = 1f;
    }
}
