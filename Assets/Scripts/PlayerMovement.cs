using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField] float jumpForce = 400;
    [SerializeField] float groundCheckRadius = 1;
    [SerializeField] float groundCheckDistance = 0;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float moveAccel = 1;

    [SerializeField] float maxMoveSpeed = 5;
    [SerializeField] float minMoveSpeed = 0.5f;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            Jump();
        }
        if (Input.GetKey(KeyCode.A) && IsGrounded()) {
            ScrollObject.currentScrollMult -= moveAccel * Time.deltaTime;
            ScrollObject.currentScrollMult = Mathf.Clamp(ScrollObject.currentScrollMult, minMoveSpeed, maxMoveSpeed);
        }
        if (Input.GetKey(KeyCode.D) && IsGrounded()) {
            ScrollObject.currentScrollMult += moveAccel * Time.deltaTime;
            ScrollObject.currentScrollMult = Mathf.Clamp(ScrollObject.currentScrollMult, minMoveSpeed, maxMoveSpeed);
        }
    }

    void Jump() {
        rigidBody.AddForce(Vector3.up * jumpForce);
    }

    bool IsGrounded()
    {
        if (Physics2D.CircleCast(transform.position, groundCheckRadius, -transform.up, groundCheckDistance, groundLayer))
        {
            return true;
        }
        return false;
    }
}
