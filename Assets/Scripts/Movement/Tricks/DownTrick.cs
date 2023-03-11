using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrick : Trick
{
    SpriteRenderer spriteRenderer;
    Sprite gottemSprite;
    Sprite playerSprite;

    public DownTrick(SpriteRenderer spriteRenderer, Sprite gottemSprite)
    {
        this.spriteRenderer = spriteRenderer;
        this.gottemSprite = gottemSprite;
    }

    public override void DuringTrick()
    {

    }

    public override void EndTrick()
    {
        spriteRenderer.sprite = playerSprite;
    }

    public override void StartTrick()
    {
        playerSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = gottemSprite;
    }
}
