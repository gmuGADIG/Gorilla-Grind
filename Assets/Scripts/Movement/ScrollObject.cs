using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public static Quaternion MoveAngle;
    private const float baseScrollSpeed = 1;

    [Tooltip("Change based on which distance layer the object is on. Further away moves slower, closer moves faster.")]
    [SerializeField] float scrollRate = 1f;

    void FixedUpdate()
    {
        transform.position -= PlayerMovement.CurrentDirection * (PlayerMovement.CurrentSpeed * baseScrollSpeed * scrollRate * Time.deltaTime);
    }

    void OnBecameInvisible() {
        //gameObject.SetActive(false);
    }

}
