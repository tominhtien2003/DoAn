using UnityEngine;

public class SkeletonEnemy : BaseEnemy , IPatrolEnemy
{
    [SerializeField] private Transform player;
    [SerializeField] private EnemyAttackZone attackZone;
    private EnemyStateMachine stateMachine;

    public Transform pointLeftLimit, pointRightLimit;
    public float patrolIdleTime = 1f;
    private bool isInAttackZone;

    public float leftLimitPos { get; private set; }
    public float rightLimitPos { get; private set; }
    public override bool IsAttacking { get; set; }
    public float AttackCooldown = 2f;
    private float cooldownTimer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        stateMachine = new EnemyStateMachine(this);

        cooldownTimer = AttackCooldown;
        SetLeftRight();

        stateMachine.ChangeState(new PatrolState(this));
        if (attackZone != null)
            attackZone.OnPlayerZoneChanged += (s, e) => isInAttackZone = e.IsPlayerInZone;
    }

    private void FixedUpdate()
    {
        cooldownTimer += Time.fixedDeltaTime;

        if (IsHurting)
        {
            return;
        }
        if (IsAttacking) return;

        if (isInAttackZone)
            stateMachine.ChangeState(new AttackState(this));
        else if (PlayerInSight())
            stateMachine.ChangeState(new ChaseState(this));
        else
            stateMachine.ChangeState(new PatrolState(this));

        stateMachine.Update();
    }
    private void SetLeftRight()
    {
        leftLimitPos = pointLeftLimit.position.x;
        rightLimitPos = pointRightLimit.position.x;
    }
    public override EnemyType EnemyType => EnemyType.Skeleton;
    public override Transform GetPlayer() => player;
    public override bool PlayerInSight() => player.position.x >= leftLimitPos && player.position.x <= rightLimitPos;
    public override bool IsPlayerInAttackZone() => isInAttackZone;
    public override float GetAttackCooldown() => AttackCooldown;
    public override float GetCooldownTimer() => cooldownTimer;
    public override void ResetCooldown() => cooldownTimer = 0f;
    public override EnemyStateMachine GetStateMachine() => stateMachine;
    public override void OnAttackAnimationFinished()
    {
        if (stateMachine.GetCurrentState() is AttackState attackState)
            attackState.OnAttackAnimationFinished();
    }

    public float GetLeftLimit()
    {
        return leftLimitPos;
    }

    public float GetRightLimit()
    {
        return rightLimitPos;
    }

    public float GetIdleDuration()
    {
        return patrolIdleTime;
    }

    public override void ApplyDamageToPlayer()
    {
        if (player.TryGetComponent<IDamageable>(out var playerHealth))
        {
            //Debug.Log("Apply damage to player");
            playerHealth.TakeDamage(damage);
        }
    }
}
