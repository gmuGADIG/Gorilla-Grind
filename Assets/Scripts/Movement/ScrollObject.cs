using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attaching this script to an object causes it to move left based on the player's speed, simulating the player moving right.
/// Allows for different rates for a parallax effect.
/// </summary>
public class ScrollObject : MonoBehaviour
{
    public static Quaternion MoveAngle;
    private const float baseScrollSpeed = 1;

    [Tooltip("Change based on which distance layer the object is on. Further away moves slower, closer moves faster.")]
    [SerializeField] float scrollRate = 1f;

    void FixedUpdate()
    {
        transform.position += Vector3.left * (PlayerMovement.CurrentHorizontalSpeed * baseScrollSpeed * scrollRate * Time.deltaTime);
    }

    void OnBecameInvisible() {
        //gameObject.SetActive(false);
    }

}
