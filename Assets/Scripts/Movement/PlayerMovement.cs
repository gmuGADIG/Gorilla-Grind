using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Check Variables")]
    [Tooltip("How far horizontally ground checks will be performed. Should be set to a width equal to the bottom of the character.")]
    [SerializeField] float groundCheckRadius = 1;
    [Tooltip("How far vertically the ground check will be performed. Too large values allow mid-air jumping, too small values prevent jumping.")]
    [SerializeField] float groundCheckDistance = 0;
    [SerializeField] LayerMask groundLayer;
    [Tooltip("How long the player will be able to jump after leaving the ground")]
    [SerializeField] float coyoteTimeSeconds = 0.5f;
    private float coyoteTimer;

    // move adjust vars
    public static float CurrentSpeed { get; private set; }

    [Header("Player Movment Variables")]
    [SerializeField] float jumpForce = 400;
    [Tooltip("How quickly the player speeds up and slows down.")]
    [SerializeField] float movementAcceleration = 1;
    [Tooltip("The player's maximum move speed.")]
    [SerializeField] float defaultMoveSpeed = 1;
    [Tooltip("The speed the player has when they spawn.")]
    [SerializeField] float maxMoveSpeed = 5;
    [Tooltip("The player's minimum move speed.")]
    [SerializeField] float minMoveSpeed = 0.5f;
    [Tooltip("Mid-air rotation speed in degrees per second.")]
    
    [SerializeField] float rotationSpeed = 90;

    // death
    public UnityEvent OnDeath;
    public bool IsDead { get; private set; }
    [SerializeField] Collider2D headCollider;

    private Rigidbody2D rigidBody;
    private RaycastHit rayHit;

    void Start() {
        IsDead = false;
        CurrentSpeed = defaultMoveSpeed;
        rigidBody = GetComponent<Rigidbody2D>();
        coyoteTimer = coyoteTimeSeconds; // initalize coyoteTimer

        // sample death listener
        OnDeath.AddListener(() => {
            GetComponent<SpriteRenderer>().color = Color.red;
        });
    }

    void Update()
    {
        //Debug.Log($"rotation: {transform.eulerAngles.z}");
        if (IsGrounded()) {
            // move speed adjust
            if (Input.GetKey(KeyCode.A)) {
                CurrentSpeed -= movementAcceleration * Time.deltaTime;
                CurrentSpeed = Mathf.Clamp(CurrentSpeed, minMoveSpeed, maxMoveSpeed);
            }
            if (Input.GetKey(KeyCode.D)) {
                CurrentSpeed += movementAcceleration * Time.deltaTime;
                CurrentSpeed = Mathf.Clamp(CurrentSpeed, minMoveSpeed, maxMoveSpeed);
            }
            // reset coyoteTimer
            coyoteTimer = coyoteTimeSeconds;
        } else {
            // rotation in midair
            if (Input.GetKey(KeyCode.A)) {
                transform.Rotate(new Vector3(0, 0, rotationSpeed*Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.D)) {
                transform.Rotate(new Vector3(0, 0, -rotationSpeed*Time.deltaTime));
            }
            // decrement coyoteTimer
            coyoteTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimer > 0) {
            Jump();
            coyoteTimer = 0;
        }
        // set constant x position. This allows the player to go up inclines without slipping back down.
        transform.position = new Vector3(0, transform.position.y, 0);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // death on blunt force head trauma
        // the head collider is the only collider rn
        //if (collider == headCollider) { // this check doesnt work
            if (!IsDead) {
                OnDeath.Invoke();
                IsDead = true;
            }
        //}
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
