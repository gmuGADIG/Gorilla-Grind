using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script for handling player movement, including left/right inputs, jump inputs, gravity, projectile motion, and collision
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region Constants
    [Tooltip("The center-point of the skateboard")]
    [SerializeField] Transform skateboard;

    [Header("Ground Check Variables")]
    [SerializeField] LayerMask defaultGroundLayer;
    
    [Tooltip("The layer on which the player normally falls through but can grind on")]
    [SerializeField] LayerMask grindLayer;
    
    [SerializeField] float landingAngleThreshold = 30;

    [Header("Grounded Movement Variables")]
    [SerializeField] float jumpForce = 9;

    [Tooltip("How far ahead of the skateboard to look when seeing if the ground has ended")]
    [SerializeField] float groundCheckXOffset;
    
    [Tooltip("How far ahead of the skateboard to look when calculating the ground's tangent")]
    [SerializeField] float slopeCheckXOffset;

    [Header("Mid-Air Movement Variables")]
    [SerializeField] float gravity = 9.8f;

    [Tooltip("How quickly the player speeds up and slows down (meters / sec^2).")]
    [SerializeField] float movementAcceleration = 3;

    [Tooltip("The fastest speed the player can reach with basic movement.")]
    [SerializeField] float maxMoveSpeed = 20;
    
    [SerializeField] float minMoveSpeed = 0.5f;
    
    [Tooltip("Mid-air rotation speed in degrees per second.")]
    [SerializeField] float rotationSpeed = 90;
    #endregion
    
    LayerMask currentGroundLayer;
    Vector2 velocity;
    PlayerMovementState currentMoveState = PlayerMovementState.Grounded;
    float lastJumpTime = 0f;
    
    /// <summary>
    /// The horizontal component of the player's current velocity.
    /// (Internally, velocity should be used. Each frame this is set based on velocity)
    /// </summary>
    public static float CurrentHorizontalSpeed { get; private set; }

    // death
    public UnityEvent OnDeath;
    public bool IsDead { get; private set; } = false;
    
    void Start()
    {
        currentGroundLayer = defaultGroundLayer;

        // sample death listener
        OnDeath.AddListener(() => {
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
        });
    }
    
    // Movement uses physics so it must be in FixedUpdate
    void FixedUpdate()
    { 
        if (currentMoveState == PlayerMovementState.Grounded) {
            // find the ground below the board, at 3 different offsets
            var (midHit, midPoint) = GroundCast(0);
            var (slopeCheckHit, slopeCheckPoint)  = GroundCast(slopeCheckXOffset);
            var (groundCheckHit, _) = GroundCast(groundCheckXOffset);

            // if any casts fail to find the ground, exit grounded state
            if (!midHit || !slopeCheckHit || !groundCheckHit)
            {
                ExitGroundedState();
                return;
            }
            
            Vector2 groundTangent = (slopeCheckPoint - midPoint).normalized;

            // adjust velocity based on input. accelerating this way can only go so fast, but speed is never hard capped
            if (velocity == Vector2.zero) 
            {
                velocity = groundTangent * minMoveSpeed; // correct for normalized zero vector still being zero
            }
            if (Input.GetKey(KeyCode.A))
            {
                float newSpeed = velocity.magnitude - movementAcceleration * Time.deltaTime;
                newSpeed = Mathf.Clamp(newSpeed, minMoveSpeed, 10000); // 10000 is an arbitrary high number
                velocity = velocity.normalized * newSpeed;
            }
            if (Input.GetKey(KeyCode.D)) {
                float newSpeed = velocity.magnitude + movementAcceleration * Time.deltaTime;
                newSpeed = Mathf.Clamp(newSpeed, minMoveSpeed, maxMoveSpeed);
                if (newSpeed > velocity.magnitude)
                {
                    // ignore if it would slow player down
                    velocity = velocity.normalized * newSpeed;
                }
            }

            // redirect magnitude to be parallel to the ground
            velocity = velocity.magnitude * groundTangent;
            
            // apply gravity (speed up when going down a slope)
            float gravityStrength = -groundTangent.normalized.y;
            if (gravityStrength > 0) velocity += Vector2.down * (gravityStrength * gravity * Time.deltaTime);

            // move player such that skateboard is exactly on ground
            // (only change height here. horizontal position is handled in ScrollObject) 
            transform.position = new Vector3(transform.position.x, midPoint.y - skateboard.localPosition.y, transform.position.z);
        }
        else if (currentMoveState == PlayerMovementState.InAir)
        {
            // check for landing. TODO: check for landing better. what if player jumps and immediately hits the ground again?
            float timeSinceLastJump = Time.time - lastJumpTime;
            if (timeSinceLastJump > 0.5f && LandingCheck())
                EnterGroundedState();
            
            // rotate based on inputs
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(new Vector3(0, 0, 1), -rotationSpeed * Time.deltaTime);
            }
            
            // add gravity and adjust position
            velocity += Vector2.down * (gravity * Time.deltaTime);
            transform.position += Vector3.up * (velocity.y * Time.deltaTime); // again, horizontal position is handled in Scrollobject
        }
        
        
        CurrentHorizontalSpeed = velocity.x;
    }

    // Events that trigger on key down must be handled in Update
    void Update()
    {
        if (currentMoveState == PlayerMovementState.Grounded)
        {
            // on space, jump (leave grounded state and apply upward force)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (velocity.y < 0)
                {
                    velocity.y = 0; // if moving down, reset speed or jumps will feel weird. might change how this is handled in the future
                }
                velocity += Vector2.up * jumpForce;
                ExitGroundedState();
            }
        }
        else if (currentMoveState == PlayerMovementState.InAir)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AttemptStartGrind();
            }
        }
        
        UpdateSpriteRotation();
    }


    /// <summary>
    /// Casts a vertical line from the player (plus the given offset) and returns the highest hit.
    /// if no hit is found, `hit` will be false, and `point` Should Not be Read!
    /// </summary>
    (bool hit, Vector2 point) GroundCast(float xOffset)
    {
        Vector3 origin = skateboard.position + new Vector3(xOffset, 10);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 20, currentGroundLayer);
        if (hit.collider is null)
        {
            return (false, Vector2.zero);
        }
        else
        {
            return (true, hit.point);
        }
    }

    bool LandingCheck()
    {
        return Physics2D.CircleCast(skateboard.position, .1f, Vector2.up, 0.5f, currentGroundLayer);
    }

    void ExitGroundedState()
    {
        lastJumpTime = Time.time;
        currentMoveState = PlayerMovementState.InAir;
        currentGroundLayer = defaultGroundLayer;
        Debug.Log("Exiting grounded state");
    }

    void EnterGroundedState()
    {
        this.currentMoveState = PlayerMovementState.Grounded;
        Debug.Log("Entering grounded state");
        
        // Get ground's tangent
        var (midHit, midPoint) = GroundCast(0);
        var (slopeCheckHit, slopeCheckPoint)  = GroundCast(slopeCheckXOffset);
        Vector2 groundTangent = (slopeCheckPoint - midPoint).normalized;
        float groundAngle = Vector2.SignedAngle(Vector2.right, groundTangent);
        
        // If tangent is too far from current rotation, the player loses
        float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(groundAngle, transform.eulerAngles.z));
        if (deltaAngle > landingAngleThreshold)
        {
            if (IsDead) return;
            OnDeath.Invoke();
            IsDead = true;
        }
        
        // Calculate new speed. faster if velocity is parallel to ground. lose some speed otherwise
        velocity = Vector2.Lerp(Vector3.Project(velocity, groundTangent), velocity, 0.5f);
        
        Debug.Log($"deltaAngle = {deltaAngle}");
    }

    void AttemptStartGrind()
    {
        if (Physics2D.CircleCast(this.skateboard.position, 1f, Vector2.down, 0f, grindLayer))
        {
            this.currentGroundLayer = grindLayer;
            EnterGroundedState();
        }
    }

    void UpdateSpriteRotation()
    {
        if (currentMoveState == PlayerMovementState.Grounded)
        {
            float angle = Vector2.SignedAngle(Vector2.right, this.velocity);
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 15);
        }
    }
}

enum PlayerMovementState
{
    Grounded, InAir
}