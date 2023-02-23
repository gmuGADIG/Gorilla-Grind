using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField] float jumpForce = 400;
    [SerializeField] float radius = 1f;
    [SerializeField] float distance = 0f;
    [SerializeField] LayerMask groundLayer;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            Jump();
        }
    }

    void Jump() {
        rigidBody.AddForce(Vector3.up * jumpForce);
    }

    bool IsGrounded()
    {
        if (Physics2D.CircleCast(transform.position, radius, -transform.up, distance, groundLayer))
        {
            return true;
        }
        return false;
    }
}
