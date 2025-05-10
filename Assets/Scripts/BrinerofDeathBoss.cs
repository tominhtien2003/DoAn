using UnityEngine;
using System.Collections;

public class BrinerofDeathBoss : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator brinerofDeathBossAnimator;
    [SerializeField] private Transform player;
    [SerializeField] private EnemyAttackZone meleeAttackZone;
    [SerializeField] public Rigidbody2D rb;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Attack Settings")]
    [SerializeField] private float meleeAttackRange = 5f;
    [SerializeField] private float darkOrbRange = 8f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject darkOrbPrefab;
    [SerializeField] private int orbsPerAttack = 3;
    [SerializeField] private float orbSpawnDelay;

    [Header("Teleport Settings")]
    [SerializeField] private float teleportCooldown = 5f;
    [SerializeField] private float damageThresholdForTeleport = 100f;
    [SerializeField] private Transform[] teleportPoints;
    [SerializeField] private float teleportDelay = 0.5f;
    [SerializeField] private GameObject teleportEffect;

    // State Machine
    private BrinerofDeathStateMachine stateMachine;
    private bool isInMeleeAttackZone = false;
    private float cooldownTimer;
    public bool IsDroppingDarkOrb = false;
    private float teleportTimer;
    private float damageSinceLastTeleport;
    private bool isTeleporting;

    public bool IsAttacking { get; set; } = false;
    public bool CanAttack => cooldownTimer >= attackCooldown;

    private void Awake()
    {
        stateMachine = new BrinerofDeathStateMachine(this);
    }

    private void Start()
    {
        cooldownTimer = attackCooldown;
        teleportTimer = teleportCooldown;
        if (meleeAttackZone != null)
            meleeAttackZone.OnPlayerZoneChanged += MeleeAttackZone_OnPlayerZoneChanged;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        teleportTimer += Time.deltaTime;
        UpdateState();
    }

    private void UpdateState()
    {
        if (IsAttacking) return;

        if (CanAttack)
        {
            if (CanMeleeAttack())
            {
                stateMachine.ChangeState(new BrinerofDeathMeleeAttackState(this));
            }
            else if (IsPlayerInDarkOrbRange())
            {
                stateMachine.ChangeState(new BrinerofDeathDarkOrbState(this));
            }
            else if (IsPlayerInMeleeRange())
            {
                stateMachine.ChangeState(new BrinerofDeathChaseState(this));
            }
            else
            {

            }
        }
        else if (!CanMeleeAttack() && IsPlayerInMeleeRange())
        {
            stateMachine.ChangeState(new BrinerofDeathChaseState(this));
        }
        else
        {
            
        }

        stateMachine.Update();
    }

    private IEnumerator Teleport()
    {
        isTeleporting = true;
        if (teleportEffect != null)
            Instantiate(teleportEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(teleportDelay);

        // Select random teleport point
        if (teleportPoints != null && teleportPoints.Length > 0)
        {
            Transform targetPoint = teleportPoints[Random.Range(0, teleportPoints.Length)];
            transform.position = targetPoint.position;
        }

        if (teleportEffect != null)
            Instantiate(teleportEffect, transform.position, Quaternion.identity);

        // Reset teleport conditions
        damageSinceLastTeleport = 0f;
        teleportTimer = 0f;
        isTeleporting = false;
    }

    public void SpawnDarkOrbs()
    {
        StartCoroutine(SpawnOrbsSequence());
    }

    private IEnumerator SpawnOrbsSequence()
    {
        float orbHeightOffset = 2f; 
        float predictionDistance = .5f; 

        for (int i = 0; i < orbsPerAttack; i++)
        {
            Vector2 predictedPosition = (Vector2)player.position;
            
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null && Mathf.Abs(playerRb.linearVelocity.x) > 0.1f)
            {
                float playerDirection = player.localScale.x > 0 ? 1f : -1f;
                predictedPosition.x += playerDirection * predictionDistance * (i + 1);
            }
            
            predictedPosition.y += orbHeightOffset; 

            GameObject orb = Instantiate(darkOrbPrefab, new Vector3(predictedPosition.x, predictedPosition.y, 0), Quaternion.identity);
            
            yield return new WaitForSeconds(orbSpawnDelay);
        }

        yield return new WaitForSeconds(attackCooldown);
        IsAttacking = false;
        ResetCooldown();
        IsDroppingDarkOrb = false;
    }

    #region State Machine Interface
    public bool IsPlayerInMeleeRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= meleeAttackRange;
    }

    public bool IsPlayerInDarkOrbRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= darkOrbRange && distanceToPlayer > meleeAttackRange;
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
    public float GetMoveSpeed() => moveSpeed;
    public float GetAttackCooldown() => attackCooldown;
    public float GetCooldownTimer() => cooldownTimer;
    public void ResetCooldown() => cooldownTimer = 0f;
    public bool CanMeleeAttack() => isInMeleeAttackZone;
    public bool IsInMeleeAttackZone() => isInMeleeAttackZone;
    #endregion

    private void MeleeAttackZone_OnPlayerZoneChanged(object sender, EnemyAttackZone.PlayerZoneChangedEventArgs e)
    {
        isInMeleeAttackZone = e.IsPlayerInZone;
    }
    public void ApplyDamageToPlayerByMeleeAttack()
    {
        if (player.TryGetComponent<IDamageable>(out var playerHealth))
        {
            playerHealth.TakeDamage(15);
        }
    }

    private void OnDrawGizmos()
    {
        // Melee Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        // Dark Orb Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, darkOrbRange);
    }
}
