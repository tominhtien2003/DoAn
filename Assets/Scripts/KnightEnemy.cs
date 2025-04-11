using UnityEngine;

public class KnightEnemy : BaseEnemy
{
    private IEnemyState currentState;
    [SerializeField] private Transform player;
    [SerializeField] private KnightAttackZone attackZone;
    public Transform pointLeftLimit, pointRightLimit;
    public float leftLimitPos { get; set; }
    public float rightLimitPos { get; set; }
    public float patrolIdleTime = 1f;

    public bool IsAttacking { get; set; }
    public float AttackCooldown = 2f;
    public float CooldownTimer { get; set; }

    private bool isInAttackZone;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        leftLimitPos = pointLeftLimit.position.x;
        rightLimitPos = pointRightLimit.position.x;

        CooldownTimer = AttackCooldown;

        ChangeState(new PatrolState(this, leftLimitPos, rightLimitPos, patrolIdleTime));
        if (attackZone != null)
        {
            attackZone.OnPlayerZoneChanged += AttackZone_OnPlayerZoneChanged;
        }
    }

    private void AttackZone_OnPlayerZoneChanged(object sender, KnightAttackZone.PlayerZoneChangedEventArgs e)
    {
        isInAttackZone = e.IsPlayerInZone;
    }
    private void FixedUpdate()
    {
        CooldownTimer += Time.fixedDeltaTime;

        if (isInAttackZone)
        {
            if (!(currentState is AttackState))
            {
                ChangeState(new AttackState(this));
            }
        }
        else if (PlayerInSight() && !IsAttacking)
        {
            if (!(currentState is ChaseState))
            {
                ChangeState(new ChaseState(this));
            }
        }
        else
        {
            if (!(currentState is PatrolState))
            {
                ChangeState(new PatrolState(this, leftLimitPos, rightLimitPos, patrolIdleTime));
            }
        }

        currentState?.Execute();
    }
    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public bool PlayerInSight()
    {
        if (player.position.x >= leftLimitPos && player.position.x <= rightLimitPos)
        {
            return true;
        }
        return false;
    }
    public bool IsPlayerInAttackZone()
    {
        return attackZone != null && attackZone.isPlayerInZone;
    }
    public void OnAttackAnimationFinished()
    {
        if (currentState is AttackState attackState)
        {
            attackState.OnAttackAnimationFinished();
        }
    }
    public Transform GetPlayer() => player;
}
