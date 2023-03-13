using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script for handling player movement, including left/right inputs, jump inputs, gravity, projectile motion, and collision
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("The center-point of the skateboard")]
    [SerializeField] Transform skateboardCenter;
    [SerializeField] Transform skateboardTransform;
    [SerializeField] Transform gorillaTransform;

    #region GroundCheckVariables
    [Header("Ground Check Variables")]
    [Tooltip("LayerMask for ground objects (not including grindable objects).")]
    [SerializeField] LayerMask groundLayer;
    
    [Tooltip("LayerMask for all grindable objects. Player will fall through these objects if not actively grinding.")]
    [SerializeField] LayerMask grindLayer;

    [Tooltip("How far ahead of the skateboard to look when seeing if the ground has ended")]
    [SerializeField] float groundCheckXOffset;

    [Tooltip("How far ahead of the skateboard to look when calculating the ground's tangent")]
    [SerializeField] float slopeCheckXOffset;

    [Tooltip("Midpoint of the skateboard, used for calculating ground tangent.")]
    [SerializeField] float midPointOffset = 0;

    [Tooltip("If the player lands at an angle above this threshold, they will die.")]
    [SerializeField] float deathLandingAngleThreshold = 30;
    #endregion

    #region GroundedMovementVariables
    [Header("Grounded Movement Variables")]
    [Tooltip("Max velocity change when jumping. Measured in meters / sec.")]
    [SerializeField] float maxJumpVelocity = 9;
    [Tooltip("How quickly jump velocity charges when spacebar is held.")]
    [SerializeField] float jumpChargeSpeed = 1f;
    [Tooltip("How quickly the player speeds up and slows down (meters / sec^2).")]
    [SerializeField] float movementAcceleration = 3;

    [Tooltip("The fastest speed the player can reach with basic ground movement. Does not account for movement from slopes/gravity.")]
    [SerializeField] float maxMoveSpeed = 20;

    [Tooltip("The minimum speed the player can move. Set to 0 to allow the player to stand still.")]
    [SerializeField] float minMoveSpeed = 0.5f;
    #endregion

    #region MidAirMovementVariables
    [Header("Mid-Air Movement Variables")]
    [Tooltip("The base/default gravity value.")]
    [SerializeField] float baseGravity = 9.8f;
    [Tooltip("Mid-air rotation speed in degrees per second.")]
    [SerializeField] float rotationSpeed = 90;
    [Tooltip("Player's gravity change when performing tricks. g_trick = g_default + trickGravityOffset.")]
    [SerializeField] float trickGravityOffset = -10f;
    [Tooltip("Player's gravity change when fast falling (holding S in midair).")]
    [SerializeField] float fastFallGravityIncrease = 10f;
    #endregion

    /// <summary>
    /// The horizontal component of the player's current velocity.
    /// (Internally, velocity should be used. Each frame this is set based on velocity)
    /// </summary>
    public static float CurrentHorizontalSpeed { get; private set; }
    public float CurrentJumpVelocity => currentJumpVelocity;
    public float MaxJumpVelocity => maxJumpVelocity;

    LayerMask currentSkateableLayer;
    Vector2 velocity;
    float currentJumpVelocity;
    float currentGravity;
    PlayerMovementState currentMoveState = PlayerMovementState.Grounded;
    float groundCheckCooldownAfterJump = .2f;

    // the current trick the player is performing.
    Trick currentPlayerTrick = null;

    // list of tricks available to the player. Stored/accessed using the Trick's class type.
    Dictionary<Type, Trick> availableTricks = new Dictionary<Type, Trick>();
    float lastJumpTime = 0f;
    
    // death
    public UnityEvent OnDeath;
    public bool IsDead { get; private set; } = false;
    
    void Start()
    {
        currentSkateableLayer = groundLayer;
        currentGravity = baseGravity;
        // sample death listener
        OnDeath.AddListener(() => {
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
        });
        availableTricks.Add(typeof(UpTrick), new UpTrick(skateboardTransform));
        availableTricks.Add(typeof(LeftTrick), new LeftTrick(skateboardTransform));
        availableTricks.Add(typeof(RightTrick), new RightTrick(skateboardTransform));
        availableTricks.Add(typeof(DownTrick), new DownTrick(skateboardTransform, gorillaTransform));
    }
    
    // Movement uses physics so it must be in FixedUpdate
    void FixedUpdate()
    { 
        if (currentMoveState == PlayerMovementState.Grounded)
        {
            DuringGrounded();
        }
        else if (currentMoveState == PlayerMovementState.InAir)
        {
            DuringInAir();
        }
        else if (currentMoveState == PlayerMovementState.TrickStance)
        {
            DuringInAir();
            DuringTrickStance();
        }
        
        CurrentHorizontalSpeed = velocity.x;
    }

    // Events that trigger on key down must be handled in Update
    void Update()
    {
        if (currentMoveState == PlayerMovementState.Grounded)
        {
            // charge jump velocity if spacebar is down
            if (Input.GetKey(KeyCode.Space))
            {
                currentJumpVelocity += (maxJumpVelocity * jumpChargeSpeed) * Time.deltaTime;
                currentJumpVelocity = Mathf.Clamp(currentJumpVelocity, 0, maxJumpVelocity);
            }
            // jump when spacebar is released
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Jump();
            }
            AdjustRotationToSlope();
        }
        else if (currentMoveState == PlayerMovementState.InAir)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //AttemptStartGrind();
                EnterTrickState();
            }
            
            // fast fall
            if (Input.GetKeyDown(KeyCode.S))
            {
                currentGravity = baseGravity + fastFallGravityIncrease;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                currentGravity = baseGravity;
            }
        }
        else if (currentMoveState == PlayerMovementState.TrickStance)
        {
            DuringTrickStance();
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ExitTrickState();
            }
        }
        if (currentMoveState != PlayerMovementState.TrickStance && currentPlayerTrick != null)
        {
            ExitTrickState();
        }
    }

    void EnterTrickState()
    {
        currentGravity = baseGravity + trickGravityOffset;
        currentMoveState = PlayerMovementState.TrickStance;
    }

    void ExitTrickState()
    {
        currentGravity = baseGravity;
        if (currentPlayerTrick != null)
        {
            currentPlayerTrick.EndTrick();
            currentPlayerTrick = null;
        }
        currentMoveState = PlayerMovementState.InAir;
    }

    void Jump()
    {
        if (velocity.y < 0)
        {
            velocity.y = 0; // if moving down, reset speed or jumps will feel weird. might change how this is handled in the future
        }
        velocity += Vector2.up * currentJumpVelocity;
        ExitGroundedState();
        currentJumpVelocity = 0f;
    }

    void DuringGrounded()
    {
        // find the ground below the board, at 3 different offsets
        (bool midHit, Vector2 midPoint) = GroundCast(midPointOffset);
        (bool slopeCheckHit, Vector2 slopeCheckPoint) = GroundCast(slopeCheckXOffset);
        (bool groundCheckHit, Vector2 _) = GroundCast(groundCheckXOffset);

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
        if (Input.GetKey(KeyCode.D))
        {
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
        if (gravityStrength > 0) velocity += Vector2.down * (gravityStrength * currentGravity * Time.deltaTime);

        // move player such that skateboard is exactly on ground
        // (only change height here. horizontal position is handled in ScrollObject) 
        transform.position = new Vector3(transform.position.x, midPoint.y - skateboardCenter.localPosition.y, transform.position.z);
    }

    void DuringInAir()
    {
        // check for landing. TODO: check for landing better. what if player jumps and immediately hits the ground again?
        float timeSinceLastJump = Time.time - lastJumpTime;
        if (timeSinceLastJump > groundCheckCooldownAfterJump && LandingCheck())
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
        velocity += Vector2.down * (currentGravity * Time.deltaTime);
        transform.position += Vector3.up * (velocity.y * Time.deltaTime); // again, horizontal position is handled in Scrollobject
    }

    /// <summary>
    /// Executed while the player is in trick stance. Trick stance means the player is performing a trick or is ready to start one.
    /// </summary>
    void DuringTrickStance()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeTrickType(typeof(UpTrick));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeTrickType(typeof(LeftTrick));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeTrickType(typeof(RightTrick));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeTrickType(typeof(DownTrick));
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
        {
            ChangeTrickType(null);
        }
        if (currentPlayerTrick != null)
        {
            currentPlayerTrick.DuringTrick();
        }
    }

    /// <summary>
    /// switches the player's current trick to a new trick.
    /// </summary>
    /// <param name="trickType">The class type of the new trick.</param>
    void ChangeTrickType(Type trickType)
    {
        if (currentPlayerTrick != null)
        {
            currentPlayerTrick.EndTrick();
            currentPlayerTrick = null;
            currentGravity = baseGravity;
        }
        if (trickType != null)
        {
            currentPlayerTrick = availableTricks[trickType];
            currentPlayerTrick.StartTrick();
        }
    }

    /// <summary>
    /// Casts a vertical line from the player (plus the given offset) and returns the highest hit.
    /// if no hit is found, `hit` will be false, and `point` Should Not be Read!
    /// </summary>
    (bool hit, Vector2 point) GroundCast(float xOffset)
    {
        Vector3 origin = skateboardCenter.position + new Vector3(xOffset, 10);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 20, currentSkateableLayer);
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
        return Physics2D.CircleCast(skateboardCenter.position, .1f, Vector2.up, 0.5f, currentSkateableLayer);
    }

    void ExitGroundedState()
    {
        lastJumpTime = Time.time;
        currentMoveState = PlayerMovementState.InAir;
        currentSkateableLayer = groundLayer;
        Debug.Log("Exiting grounded state");
    }

    void EnterGroundedState()
    {
        currentMoveState = PlayerMovementState.Grounded;
        currentGravity = baseGravity;
        currentJumpVelocity = 0;
        Debug.Log("Entering grounded state");
        
        // Get ground's tangent
        (bool midHit, Vector2 midPoint) = GroundCast(midPointOffset);
        (bool slopeCheckHit, Vector2 slopeCheckPoint)  = GroundCast(slopeCheckXOffset);
        Vector2 groundTangent = (slopeCheckPoint - midPoint).normalized;
        float groundAngle = Vector2.SignedAngle(Vector2.right, groundTangent);
        
        // If tangent is too far from current rotation, the player loses
        float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(groundAngle, transform.eulerAngles.z));
        if (deltaAngle > deathLandingAngleThreshold)
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
        if (Physics2D.CircleCast(skateboardCenter.position, 1f, Vector2.down, 0f, grindLayer))
        {
            currentSkateableLayer = grindLayer;
            EnterGroundedState();
        }
    }

    void AdjustRotationToSlope()
    {
        float angle = Vector2.SignedAngle(Vector2.right, velocity);
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 15);
    }
}

enum PlayerMovementState
{
    Grounded, InAir, TrickStance
}