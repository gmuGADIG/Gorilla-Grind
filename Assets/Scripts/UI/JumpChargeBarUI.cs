using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
