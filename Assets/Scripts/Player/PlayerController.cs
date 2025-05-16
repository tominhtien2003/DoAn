using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerAttackZone;
using System.Collections;

public class PlayerController : BasePlayer
{
    [Header("Components")]
    [SerializeField] private PlayerAttackZone attackZone;

    [Header("Jump Settings")]
    [SerializeField] private int maxJumps = 2;
    private int jumpsRemaining;
    private bool wasGrounded;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float postDashDelay = 0.01f;

    [Header("Wall Slide Settings")]
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float wallSlideGravity = 0.5f;

    // Movement states
    private bool isMoving;
    private bool isDashing;
    private bool canDash = true;
    public bool IsMovementLocked { get; private set; }

    // Timers
    private float dashTimeLeft;
    private float dashCooldownTimeLeft;
    private float postDashDelayTimeLeft;

    // Combat
    private bool isInAttackZone;
    private IDamageable currentEnemy;
    private TouchingDirection touchingDirection;
    private float originalGravityScale;
    public bool CanAttack { get; set; } = true;
    public bool IsAttacking { get; set; } = false;
    public bool IsRunningAnimation { get; set; } = false;
    [SerializeField] private float attackCooldown = 1f;

    private bool isWallSliding;

    #region Properties
    public bool IsMoving
    {
        get => isMoving;
        private set
        {
            if (value != isMoving)
            {
                isMoving = value;
                playerAnimator.SetBool(AnimationUtilities.IS_MOVING, value);
            }
        }
    }
    #endregion

    #region Unity Lifecycle
    public override void Awake()
    {
        base.Awake();
        touchingDirection = GetComponent<TouchingDirection>();
        originalGravityScale = rb.gravityScale;
        jumpsRemaining = maxJumps;
        wasGrounded = true;
    }

    private void Start()
    {
        if (attackZone != null)
        {
            attackZone.OnEnemyZoneChanged += HandleEnemyZoneChanged;
        }
    }

    private void Update()
    {
        HandleDashCooldown();
        HandleDashDuration();
        HandlePostDashDelay();
        HandleWallSlide();
        playerAnimator.SetFloat(AnimationUtilities.Y_VELOCITY, rb.linearVelocity.y);

        // Reset jumps only when landing on ground
        if (touchingDirection.IsGround && !wasGrounded)
        {
            jumpsRemaining = maxJumps;
        }
        wasGrounded = touchingDirection.IsGround;

        if (Mathf.Abs(rb.linearVelocity.x) < 0.1f || 
            !touchingDirection.IsGround || 
            isDashing || 
            IsAttacking || 
            IsMovementLocked)
        {
            IsRunningAnimation = false;
            AudioManager.Instance.StopSound("Run");
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    #endregion

    #region Movement
    private void HandleMovement()
    {
        if (IsMovementLocked) return;

        if (isDashing)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(transform.localScale.x) * dashSpeed, 0f);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void Flip()
    {
        if (moveDirection.x != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(moveDirection.x) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }
    #endregion

    #region Input Handlers
    public void OnTab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            foreach (var inventoryPanel in InventoryManager.Instance.inventoryPanels)
            {
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            }
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            IsMoving = false;
            moveDirection = Vector2.zero;
        }
        //if (isDashing) return;
        moveDirection = context.ReadValue<Vector2>();
        IsMoving = moveDirection != Vector2.zero;
        Flip();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && jumpsRemaining > 0 && !isDashing && !IsAttacking)
        {
            jumpsRemaining--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
            AudioManager.Instance.PlaySound("Jump");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !IsAttacking && CanAttack && !isDashing)
        {
            IsAttacking = true;
            moveDirection = Vector2.zero;
            IsMoving = false;
            LockMovement();
            playerAnimator.SetTrigger(AnimationUtilities.ATTACK);
        }
    }

    public IEnumerator AttackCooldown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        CanAttack = true;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash && !isDashing && !IsMovementLocked && !IsAttacking)
        {
            StartDash();
        }
    }
    #endregion

    #region Dash
    private void HandleDashCooldown()
    {
        if (dashCooldownTimeLeft > 0)
        {
            dashCooldownTimeLeft -= Time.deltaTime;
            if (dashCooldownTimeLeft <= 0)
            {
                canDash = true;
            }
        }
    }

    private void HandleDashDuration()
    {
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
    }

    private void HandlePostDashDelay()
    {
        if (postDashDelayTimeLeft > 0)
        {
            postDashDelayTimeLeft -= Time.deltaTime;
            if (postDashDelayTimeLeft <= 0)
            {
                UnlockMovement();
            }
        }
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        dashCooldownTimeLeft = dashCooldown;
        rb.gravityScale = 0f;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Weapon"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Trap"), true);
        playerAnimator.SetBool(AnimationUtilities.IS_DASHING, true);
    }

    private void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravityScale;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Weapon"), false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Trap"), false);
        postDashDelayTimeLeft = postDashDelay;
        //LockMovement();
        playerAnimator.SetBool(AnimationUtilities.IS_DASHING, false);
        //moveDirection = Vector2.zero;
    }
    #endregion

    #region Movement Control
    public void LockMovement()
    {
        IsMovementLocked = true;
        rb.linearVelocity = Vector2.zero;
    }

    public void UnlockMovement()
    {
        IsMovementLocked = false;
    }
    #endregion

    #region Combat
    private void HandleEnemyZoneChanged(object sender, EnemyZoneChangedEventArgs e)
    {
        isInAttackZone = e.IsEnemyInZone;
        currentEnemy = e.EnemyObject?.transform.parent.GetComponent<IDamageable>();
    }

    public bool IsEnemyInAttackZone()
    {
        return isInAttackZone;
    }

    public void ApplyDamageToEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.TakeDamage(PlayerStats.Instance.Damage);
        }
    }
    #endregion

    private void HandleWallSlide()
    {
        if (touchingDirection.IsWall && !touchingDirection.IsGround && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
            rb.gravityScale = wallSlideGravity;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
        else
        {
            if (isWallSliding)
            {
                isWallSliding = false;
                rb.gravityScale = originalGravityScale;
            }
        }
    }

    public void ResetStates()
    {
        // Reset movement states
        isMoving = false;
        isDashing = false;
        canDash = true;
        IsMovementLocked = false;
        moveDirection = Vector2.zero;

        // Reset combat states
        IsAttacking = false;
        CanAttack = true;

        // Reset physics
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravityScale;

        // Reset animation states
        playerAnimator.SetBool(AnimationUtilities.IS_MOVING, false);
        playerAnimator.SetBool(AnimationUtilities.IS_DASHING, false);
        playerAnimator.SetFloat(AnimationUtilities.Y_VELOCITY, 0f);
    }
}
