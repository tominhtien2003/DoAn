using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BasePlayer
{
    private TouchingDirection touchingDirection;

    private bool isMoving = false;
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        private set
        {
            isMoving = value;
            playerAnimator.SetBool(AnimationUtilities.IS_MOVING, value);
        }
    }
    private bool isRunning = false;
    
    public bool IsRunning
    {
        get
        {
            return isRunning;
        }
        private set
        {
            isRunning = value;
            playerAnimator.SetBool(AnimationUtilities.IS_RUNNING, value);
        }
    }
    public float CurrentSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirection.IsWall)
                {
                    if (touchingDirection.IsGround)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return moveSpeed;
                        }
                    }
                    else
                    {
                        return airSpeed;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        } 
    }
    public bool CanMove
    {
        get
        {
            return playerAnimator.GetBool(AnimationUtilities.CAN_MOVE);
        }
    }
    public override void Awake()
    {
        base.Awake();
        touchingDirection = GetComponent<TouchingDirection>();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * CurrentSpeed, rb.linearVelocity.y);

        playerAnimator.SetFloat(AnimationUtilities.Y_VELOCITY,rb.linearVelocity.y);
    }
    private void Flip()
    {
        if (moveDirection.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveDirection.x) * Mathf.Abs(transform.localScale.x),
                                                transform.localScale.y,
                                                transform.localScale.z);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        IsMoving = moveDirection != Vector2.zero;
        Flip();
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGround && CanMove)
        {
            playerAnimator.SetTrigger(AnimationUtilities.JUMP);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerAnimator.SetTrigger(AnimationUtilities.ATTACK);
        }
    }
}
