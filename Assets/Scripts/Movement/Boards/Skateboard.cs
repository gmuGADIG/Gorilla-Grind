using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skateboard : MonoBehaviour
{
    public abstract void ApplyBonuses(PlayerMovement movement);
    public abstract void RemoveBonuses(PlayerMovement movement);
}
