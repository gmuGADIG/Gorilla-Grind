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
    [Tooltip("Debug setting. If set to false, the player will still be controllable \"after death\". Should be set to TRUE for final game.")]
    [SerializeField] bool isMortal = true;
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
    [Tooltip("How quickly the player speeds up and slows down (meters / sec^2).")]
    [SerializeField] float movementAcceleration = 3;

    [Tooltip("The fastest speed the player can reach with basic ground movement. Does not account for movement from slopes/gravity.")]
    [SerializeField] float maxMoveSpeed = 20;

    [Tooltip("The minimum speed the player can move. Set to 0 to allow the player to stand still.")]
    [SerializeField] float minMoveSpeed = 0.5f;
    #endregion

    #region JumpVariables
    [Header("Jump Variables")]
    [Tooltip("Max velocity change when jumping. Measured in meters / sec.")]
    [SerializeField] float maxJumpVelocity = 9;
    [Tooltip("Min velocity change when jumping. Measured in meters / sec.")]
    [SerializeField] float minJumpVelocity = 3;
    [Tooltip("How quickly jump velocity charges when spacebar is held.")]
    [SerializeField] float jumpChargeSpeed = 1f;
    [Tooltip("How long after leaving the ground the player can jump for (seconds).")]
    [SerializeField] float coyoteTime = 3f;
    #endregion

    #region MidAirMovementVariables
    [Header("Mid-Air Movement Variables")]
    [Tooltip("The base/default gravity value.")]
    [SerializeField] float baseGravity = 9.8f;
    [Tooltip("Mid-air rotation speed in degrees per second.")]
    [SerializeField] float rotationSpeed = 90;
    [Tooltip("Player's gravity change when performing tricks. g_trick = g_default + trickGravityOffset.")]
    [SerializeField] float trickGravityOffset = -10f;
    [Tooltip("Time speed multiplier while the player is in trick mode")]
    [SerializeField] float trickTimeSlowModifier = .75f;
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
    float groundCheckCooldownAfterJump = .2f;
    float currentCoyoteTime;
    bool jumping; // true = player is in the air due to a jump. false = in air from falling

    // Dictionary used to store and retrieve states.
    Dictionary<Type, State> availableStates;
    // The current state the player is in.
    State currentState;

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
        currentCoyoteTime = coyoteTime;
        // sample death listener
        OnDeath.AddListener(() => {
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
        });

        availableStates = new Dictionary<Type, State>()
        {
            { typeof(GroundedState), new GroundedState(this) },
            { typeof(InAirState), new InAirState(this) },
            { typeof(TrickState), new TrickState(this) },
            { typeof(DeadState), new DeadState(this) },
        };

        currentState = availableStates[typeof(GroundedState)];
        currentState.BeforeExecution();

        availableTricks.Add(typeof(UpTrick), new UpTrick(skateboardTransform));
        availableTricks.Add(typeof(LeftTrick), new LeftTrick(skateboardTransform));
        availableTricks.Add(typeof(RightTrick), new RightTrick(skateboardTransform));
        availableTricks.Add(typeof(DownTrick), new DownTrick(skateboardTransform, gorillaTransform));
    }
    
    // Movement uses physics so it must be in FixedUpdate
    void FixedUpdate()
    {
        currentState.PhysicsUpdate();

        CurrentHorizontalSpeed = velocity.x;
    }

    // Events that trigger on key down must be handled in Update
    void Update()
    {
        currentState.UpdateState();
        // check transitions
        Type nextState = currentState.CheckForTransitions();
        // transition if needed
        if (nextState != null)
        {
            currentState.AfterExecution();
            currentState = availableStates[nextState];
            currentState.BeforeExecution();
        }
    }

    void Jump()
    {
        if (velocity.y < 0)
        {
            velocity.y = 0; // if moving down, reset speed or jumps will feel weird. might change how this is handled in the future
        }
        velocity += Vector2.up * currentJumpVelocity;
        currentJumpVelocity = 0f;
        jumping = true;
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
        Vector3 origin = skateboardCenter.position + new Vector3(xOffset, 0);
        RaycastHit2D upCast   = Physics2D.Raycast(origin, Vector2.up,   3, currentSkateableLayer);
        RaycastHit2D downCast = Physics2D.Raycast(origin, Vector2.down, 3, currentSkateableLayer);

        if (downCast.collider != null)
            return (true, downCast.point);
        else if (upCast.collider != null)
            return (true, upCast.point);
        else return (false, Vector2.zero);
    }

    bool LandingCheck()
    {
        return Physics2D.CircleCast(skateboardCenter.position, .1f, Vector2.up, 0.5f, currentSkateableLayer);
    }

    void AdjustRotationToSlope()
    {
        float angle = Vector2.SignedAngle(Vector2.right, velocity);
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 15);
    }

    abstract class State
    {
        /// <summary>
        /// Reference to the PlayerMovement script
        /// </summary>
        protected PlayerMovement move;
        protected Transform transform;
        protected GameObject gameObject;

        public State(PlayerMovement move)
        {
            this.move = move;
            transform = move.transform;
            gameObject = move.gameObject;
        }

        /// <summary>
        /// Called when entering the state. Use for state initialization.
        /// </summary>
        public abstract void BeforeExecution();
        /// <summary>
        /// Called every frame in the Update() method when state is active.
        /// </summary>
        public abstract void UpdateState();
        /// <summary>
        /// Called in FixedUpdate() when state is active. Place physics dependent calls here.
        /// </summary>
        public abstract void PhysicsUpdate();
        /// <summary>
        /// Called as the state is transitioning. Invoked before next state's BeforeExecution().
        /// </summary>
        public abstract void AfterExecution();
        /// <summary>
        /// Checks if the player needs to transition to another state.
        /// </summary>
        /// <returns>The class type of the state to transition to. ex: typeof(GroundedState). null if no transition.</returns>
        public abstract Type CheckForTransitions();
    }

    class GroundedState : State
    {
        public GroundedState(PlayerMovement move) : base(move) { }

        public override void AfterExecution()
        {
            move.lastJumpTime = Time.time;
            move.currentSkateableLayer = move.groundLayer;
            Debug.Log("Exiting grounded state");
        }

        public override void BeforeExecution()
        {
            print("Entering grounded state");
            move.currentGravity = move.baseGravity;
            move.currentJumpVelocity = 0;
            move.currentCoyoteTime = move.coyoteTime;
            move.jumping = false;

            // Get ground's tangent
            (bool _, Vector2 midPoint) = move.GroundCast(move.midPointOffset);
            (bool _, Vector2 slopeCheckPoint) = move.GroundCast(move.slopeCheckXOffset);
            Vector2 groundTangent = (slopeCheckPoint - midPoint).normalized;
            float groundAngle = Vector2.SignedAngle(Vector2.right, groundTangent);

            // If tangent is too far from current rotation, the player loses
            float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(groundAngle, transform.eulerAngles.z));
            if (deltaAngle > move.deathLandingAngleThreshold && move.isMortal)
            {
                if (move.IsDead) return;
                //move.OnDeath.Invoke();
                move.IsDead = true;
            }

            // Calculate new speed. faster if velocity is parallel to ground. lose some speed otherwise
            move.velocity = Vector2.Lerp(Vector3.Project(move.velocity, groundTangent), move.velocity, 0.5f);

            Debug.Log($"deltaAngle = {deltaAngle}");
        }

        public override void UpdateState()
        {
            move.AdjustRotationToSlope();
            // set jump velocity to minimum on first frame spacebar is down.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                move.currentJumpVelocity = move.minJumpVelocity;
            }
            // charge jump velocity if spacebar is down
            if (Input.GetKey(KeyCode.Space))
            {
                move.currentJumpVelocity += (move.maxJumpVelocity * move.jumpChargeSpeed) * Time.deltaTime;
                move.currentJumpVelocity = Mathf.Clamp(move.currentJumpVelocity, 0, move.maxJumpVelocity);
            }
            // jump when spacebar is released
            if (Input.GetKeyUp(KeyCode.Space))
            {
                move.Jump();
            }
        }

        public override void PhysicsUpdate()
        {
            // find the ground below the board, at 3 different offsets
            (bool _, Vector2 midPoint) = move.GroundCast(move.midPointOffset);
            (bool _, Vector2 slopeCheckPoint) = move.GroundCast(move.slopeCheckXOffset);
            (bool _, Vector2 _) = move.GroundCast(move.groundCheckXOffset);

            Vector2 groundTangent = (slopeCheckPoint - midPoint).normalized;

            // adjust velocity based on input. accelerating this way can only go so fast, but speed is never hard capped
            if (move.velocity == Vector2.zero)
            {
                move.velocity = groundTangent * move.minMoveSpeed; // correct for normalized zero vector still being zero
            }
            if (Input.GetKey(KeyCode.A))
            {
                float newSpeed = move.velocity.magnitude - move.movementAcceleration * Time.deltaTime;
                newSpeed = Mathf.Clamp(newSpeed, move.minMoveSpeed, 10000); // 10000 is an arbitrary high number
                move.velocity = move.velocity.normalized * newSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                float newSpeed = move.velocity.magnitude + move.movementAcceleration * Time.deltaTime;
                newSpeed = Mathf.Clamp(newSpeed, move.minMoveSpeed, move.maxMoveSpeed);
                if (newSpeed > move.velocity.magnitude)
                {
                    // ignore if it would slow player down
                    move.velocity = move.velocity.normalized * newSpeed;
                }
            }

            // redirect magnitude to be parallel to the ground
            move.velocity = move.velocity.magnitude * groundTangent;

            // apply gravity (speed up when going down a slope)
            float gravityStrength = -groundTangent.normalized.y;
            if (gravityStrength > 0) move.velocity += Vector2.down * (gravityStrength * move.currentGravity * Time.deltaTime);

            // move player such that skateboard is exactly on ground
            // (only change height here. horizontal position is handled in ScrollObject) 
            transform.position = new Vector3(transform.position.x, midPoint.y - move.skateboardCenter.localPosition.y, transform.position.z);
        }

        public override Type CheckForTransitions()
        {
            if (move.IsDead)
            {
                return typeof(DeadState);
            }
            // find the ground below the board, at 3 different offsets
            (bool midHit, Vector2 _) = move.GroundCast(move.midPointOffset);
            (bool slopeCheckHit, Vector2 _) = move.GroundCast(move.slopeCheckXOffset);
            (bool groundCheckHit, Vector2 _) = move.GroundCast(move.groundCheckXOffset);

            // if any casts fail to find the ground, exit grounded state
            if (!midHit || !slopeCheckHit || !groundCheckHit)
            {
                return typeof(InAirState);
            }

            // enter in air state when jumping
            if (Input.GetKeyUp(KeyCode.Space))
            {
                return typeof(InAirState);
            }

            return null;
        }
    }

    class InAirState : State
    {
        public InAirState(PlayerMovement move) : base(move) { }

        public override void AfterExecution()
        {

        }

        public override void BeforeExecution()
        {
            print("Entering in air state");
        }

        public override void PhysicsUpdate()
        {
            // add gravity and adjust position
            move.velocity += Vector2.down * (move.currentGravity * Time.deltaTime);
            transform.position += Vector3.up * (move.velocity.y * Time.deltaTime); // again, horizontal position is handled in Scrollobject
        }

        public override void UpdateState()
        {
            // decrease coyote time
            if (move.currentCoyoteTime > 0)
            {
                move.currentCoyoteTime -= Time.deltaTime;
            }

            // start fast fall
            if (Input.GetKeyDown(KeyCode.S))
            {
                move.currentGravity = move.baseGravity + move.fastFallGravityIncrease;
            }
            // end fast fall
            if (Input.GetKeyUp(KeyCode.S))
            {
                move.currentGravity = move.baseGravity;
            }
            // jump with coyote time
            if (Input.GetKeyUp(KeyCode.Space) && move.coyoteTime > 0 && !move.jumping)
            {
                move.Jump();
            }

            // rotate based on inputs
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(new Vector3(0, 0, 1), move.rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(new Vector3(0, 0, 1), -move.rotationSpeed * Time.deltaTime);
            }
        }

        public override Type CheckForTransitions()
        {
            // check for landing. TODO: check for landing better. what if player jumps and immediately hits the ground again?
            float timeSinceLastJump = Time.time - move.lastJumpTime;
            if (timeSinceLastJump > move.groundCheckCooldownAfterJump && move.LandingCheck())
            {
                return typeof(GroundedState);
            }

            // enter trick state
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //AttemptStartGrind();
                return typeof(TrickState);
            }
            return null;
        }
    }

    class TrickState : State
    {
        public TrickState(PlayerMovement move) : base(move) { }

        public override void AfterExecution()
        {
            move.currentGravity = move.baseGravity;
            if (move.currentPlayerTrick != null)
            {
                move.currentPlayerTrick.EndTrick();
                move.currentPlayerTrick = null;
            }
            Time.timeScale = 1;
            PostProcessingController.ChromaticAberration(false);
            PostProcessingController.ColorGrading(false);
        }

        public override void BeforeExecution()
        {
            print("Entering trick state");
            move.currentGravity = move.baseGravity + move.trickGravityOffset;
            Time.timeScale = move.trickTimeSlowModifier;
            PostProcessingController.ChromaticAberration(true);
            PostProcessingController.ColorGrading(true);
        }

        public override void PhysicsUpdate()
        {
            // add gravity and adjust position
            move.velocity += Vector2.down * (move.currentGravity * Time.deltaTime);
            transform.position += Vector3.up * (move.velocity.y * Time.deltaTime); // again, horizontal position is handled in Scrollobject
        }

        public override void UpdateState()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                move.ChangeTrickType(typeof(UpTrick));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                move.ChangeTrickType(typeof(LeftTrick));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                move.ChangeTrickType(typeof(RightTrick));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                move.ChangeTrickType(typeof(DownTrick));
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
            {
                move.ChangeTrickType(null);
            }
            if (move.currentPlayerTrick != null)
            {
                move.currentPlayerTrick.DuringTrick();
            }
        }
        
        public override Type CheckForTransitions()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                return typeof(InAirState);
            }
            float timeSinceLastJump = Time.time - move.lastJumpTime;
            if (timeSinceLastJump > move.groundCheckCooldownAfterJump && move.LandingCheck())
            {
                return typeof(GroundedState);
            }
            return null;
        }
    }

    class DeadState : State
    {
        public DeadState(PlayerMovement move) : base(move) { }

        public override void AfterExecution()
        {

        }

        public override void BeforeExecution()
        {
            print("Entering dead state");
            move.IsDead = true;
            move.OnDeath.Invoke();
        }

        public override Type CheckForTransitions()
        {
            return null;
        }

        public override void PhysicsUpdate()
        {
            move.velocity = Vector2.zero;
            (bool _, Vector2 midPoint) = move.GroundCast(move.midPointOffset);
            transform.position = new Vector3(transform.position.x, midPoint.y - move.skateboardCenter.localPosition.y, transform.position.z);
        }

        public override void UpdateState()
        {

        }
    }
}