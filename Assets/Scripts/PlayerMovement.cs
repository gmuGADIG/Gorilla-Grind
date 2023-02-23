using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    // ground check variables
    [SerializeField] float jumpForce = 400;
    [SerializeField] float groundCheckRadius = 1;
    [SerializeField] float groundCheckDistance = 0;
    [SerializeField] LayerMask groundLayer;

    // move adjust vars 
    [SerializeField] float moveAccel = 1;
    [SerializeField] float maxMoveSpeed = 5;
    [SerializeField] float minMoveSpeed = 0.5f;

    // rotate vars
    [SerializeField] float degreesPerSecond = 90;
    const float deathDegrees = 45;


    // death
    public UnityEvent OnDeath;
    public bool IsDead { get; private set; }

    void Start() {
        IsDead = false;
        rigidBody = GetComponent<Rigidbody2D>();

        // sample death listener
        OnDeath.AddListener(() => {
            GetComponent<SpriteRenderer>().color = Color.red;
        });
    }

    void Update()
    {
        Debug.Log($"rotation: {transform.eulerAngles.z}");

        if (IsGrounded()) {
            // jump
            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
            // move speed adjust
            if (Input.GetKey(KeyCode.A)) {
                ScrollObject.currentScrollMult -= moveAccel * Time.deltaTime;
                ScrollObject.currentScrollMult = Mathf.Clamp(ScrollObject.currentScrollMult, minMoveSpeed, maxMoveSpeed);
            }
            if (Input.GetKey(KeyCode.D)) {
                ScrollObject.currentScrollMult += moveAccel * Time.deltaTime;
                ScrollObject.currentScrollMult = Mathf.Clamp(ScrollObject.currentScrollMult, minMoveSpeed, maxMoveSpeed);
            }
        } else {
            // rotation
            if (Input.GetKey(KeyCode.A)) {
                transform.Rotate(new Vector3(0, 0, degreesPerSecond*Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.D)) {
                transform.Rotate(new Vector3(0, 0, -degreesPerSecond*Time.deltaTime));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
            // death on not standing straight
            if (transform.eulerAngles.z is > deathDegrees and < (360 - deathDegrees)) {
                if (IsDead) {
                    OnDeath.Invoke();
                    IsDead = true;
                }
            } else {
                transform.Rotate(new Vector3());
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
