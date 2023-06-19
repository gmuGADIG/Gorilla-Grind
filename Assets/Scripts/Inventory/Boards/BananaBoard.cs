using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaBoard : Skateboard
{
    [SerializeField] float accelerationPercentageIncrease = .5f;

    public override void ApplyBonuses(PlayerMovement movement)
    {
        movement.AccelerationMultiplier = 1 + accelerationPercentageIncrease;
    }

    public override void RemoveBonuses(PlayerMovement movement)
    {
        movement.AccelerationMultiplier = 1;
    }
}
