using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows the player's jump velocity charge as the spacebar is held down.
/// Temporary, and there are probably some performance improvements that can be made.
/// </summary>
public class JumpChargeBarUI : MonoBehaviour
{
    [SerializeField] RectTransform bar;

    PlayerMovement player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            bar.localScale = new Vector3(player.CurrentJumpVelocity / player.MaxJumpVelocity, bar.localScale.y, 1);
        }
        else
        {
            bar.localScale = Vector3.up * bar.localScale.y;
        }
    }
}
