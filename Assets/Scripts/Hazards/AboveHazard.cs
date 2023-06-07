using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for handling players jumping over a hazard.
/// Must be accompanied by a rigidbody2d and collider2d, which determines the area above the hazard.
/// </summary>
public class AboveHazard : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMovement>();
            if (player.IsDead) return;
            
            player.OnJumpedOverHazard.Invoke();
        }
    }
}
