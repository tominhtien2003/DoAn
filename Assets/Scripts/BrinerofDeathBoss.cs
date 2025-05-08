using UnityEngine;

public class BrinerofDeathBoss : MonoBehaviour
{
    [SerializeField] private Animator brinerofDeathBossAnimator;
    [SerializeField] private Transform player;
    [SerializeField] private EnemyAttackZone meleeAttackZone;
    public Rigidbody2D rb;

    [Header("Melee Attack Settings")]
    [SerializeField] private float meleeAttackRange = 1.5f;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float walkSpeed = 2f;
    private BrinerofDeathStateMachine stateMachine;
    private bool isInMeleeAttackZone = false;
    public bool IsAttacking { get; set; } = false;
    public float AttackCooldown;
    private float cooldownTimer;
    private void Awake()
    {
        stateMachine = new BrinerofDeathStateMachine(this);
    }
    private void Start()
    {
        cooldownTimer = AttackCooldown;
        if (meleeAttackZone != null)
            meleeAttackZone.OnPlayerZoneChanged += MeleeAttackZone_OnPlayerZoneChanged;
    }

    private void MeleeAttackZone_OnPlayerZoneChanged(object sender, EnemyAttackZone.PlayerZoneChangedEventArgs e)
    {
        isInMeleeAttackZone = e.IsPlayerInZone;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        //Debug.Log($"Cooldown Timer: {cooldownTimer}");
        if (IsAttacking) return;

        if (CanMeleeAttack())
            stateMachine.ChangeState(new BrinerofDeathMeleeAttackState(this));
        else if (PlayerInMeleeRange())
            stateMachine.ChangeState(new BrinerofDeathChaseState(this));
        stateMachine.Update();
    }
    public bool IsPlayerInMeleeRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= meleeAttackRange;
    }
    public void FaceDirection(float direction)
    {
        if (Mathf.Approximately(direction, 0f)) return;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction);
        transform.localScale = scale;
    }
    public Animator GetBrinerofDeathAnimator() => brinerofDeathBossAnimator;
    public Transform GetPlayer() => player;
    public BrinerofDeathStateMachine GetStateMachine() => stateMachine;
    public float GetWalkSpeed() => walkSpeed;
    public float GetAttackCooldown() => AttackCooldown;
    public float GetCooldownTimer() => cooldownTimer;
    public void ResetCooldown() => cooldownTimer = 0f;
    public bool CanMeleeAttack() => isInMeleeAttackZone == true;
    public bool IsPlayerInMeleeAttackZone() => isInMeleeAttackZone;
    public void OnMeleeAttackAnimationFinished()
    {
        if (stateMachine.GetCurrentState() is BrinerofDeathMeleeAttackState brinerofDeathMeleeAttack)
        {
            //Debug.Log("Call event when finish attack animation");
            brinerofDeathMeleeAttack.OnMeleeAttackAnimationFinished();
        }
    }
    public bool PlayerInMeleeRange()
    {
        Vector3 origin = transform.position;

        Vector3 rightEnd = origin + Vector3.right * meleeAttackRange;
        Vector3 leftEnd = origin + Vector3.left * meleeAttackRange;

        return player.position.x >= leftEnd.x && player.position.x <= rightEnd.x; 
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position;

        Vector3 rightEnd = origin + Vector3.right * meleeAttackRange;
        Gizmos.DrawLine(origin, rightEnd);

        Vector3 leftEnd = origin + Vector3.left * meleeAttackRange;
        Gizmos.DrawLine(origin, leftEnd);
    }
}
