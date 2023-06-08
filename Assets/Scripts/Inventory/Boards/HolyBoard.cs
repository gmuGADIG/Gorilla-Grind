using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyBoard : Skateboard
{
    [SerializeField] float accelerationPercentageIncrease = .5f;
    [SerializeField] float rotationPercentageIncrease = .5f;

    public override void ApplyBonuses(PlayerMovement movement)
    {
        movement.AccelerationMultiplier = 1 + accelerationPercentageIncrease;
        movement.RotationMultiplier = 1 + rotationPercentageIncrease;
    }

    public override void RemoveBonuses(PlayerMovement movement)
    {
        movement.AccelerationMultiplier = 1;
        movement.RotationMultiplier = 1;
    }
}
