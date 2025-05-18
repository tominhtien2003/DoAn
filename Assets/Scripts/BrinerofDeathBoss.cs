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
    [SerializeField] private float meleeAttackCooldown = 2f;
    [SerializeField] private float darkOrbAttackCooldown = 3f;

    [Header("Attack Settings")]
    [SerializeField] private float meleeAttackRange = 5f;
    [SerializeField] private float darkOrbRange = 8f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject darkOrbPrefab;
    [SerializeField] private int orbsPerAttack = 3;
    [SerializeField] private float orbSpawnDelay = 0.2f;
    [SerializeField] private float damage = 50f;

    // State Machine
    private BrinerofDeathStateMachine stateMachine;
    private bool isInMeleeAttackZone = false;
    private float meleeCooldownTimer;
    private float darkOrbCooldownTimer;
    public bool IsHurting;
    private bool isDroppingDarkOrb;

    public bool IsAttacking { get; set; } = false;
    public bool IsDroppingDarkOrb { get => isDroppingDarkOrb; set => isDroppingDarkOrb = value; }
    public bool CanMeleeAttack => meleeCooldownTimer >= meleeAttackCooldown;
    public bool CanDarkOrbAttack => darkOrbCooldownTimer >= darkOrbAttackCooldown;

    private void Awake()
    {
        stateMachine = new BrinerofDeathStateMachine(this);
    }

    private void Start()
    {
        meleeCooldownTimer = meleeAttackCooldown;
        darkOrbCooldownTimer = darkOrbAttackCooldown;
        if (meleeAttackZone != null)
            meleeAttackZone.OnPlayerZoneChanged += MeleeAttackZone_OnPlayerZoneChanged;
        AudioManager.Instance.PlayMusic("Music Fighting Boss");
    }

    private void Update()
    {
        meleeCooldownTimer += Time.deltaTime;
        darkOrbCooldownTimer += Time.deltaTime;
        if (IsHurting) return;
        UpdateState();
    }

    private void UpdateState()
    {
        if (IsAttacking) return;

        if (IsDroppingDarkOrb) return;

        if (IsInMeleeAttackZone() && CanMeleeAttack)
        {
            stateMachine.ChangeState(new BrinerofDeathMeleeAttackState(this));
        }
        else if (IsPlayerInDarkOrbRange() && CanDarkOrbAttack)
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

        stateMachine.Update();
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

        yield return new WaitForSeconds(darkOrbAttackCooldown);
        IsAttacking = false;
        ResetDarkOrbCooldown();
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
    public float GetAttackCooldown() => darkOrbAttackCooldown;
    public float GetCooldownTimer() => darkOrbCooldownTimer;
    public void ResetCooldown() => darkOrbCooldownTimer = 0f;
    public bool IsInMeleeAttackZone() => isInMeleeAttackZone;
    public void ResetMeleeCooldown() => meleeCooldownTimer = 0f;
    public void ResetDarkOrbCooldown() => darkOrbCooldownTimer = 0f;
    #endregion

    private void MeleeAttackZone_OnPlayerZoneChanged(object sender, EnemyAttackZone.PlayerZoneChangedEventArgs e)
    {
        isInMeleeAttackZone = e.IsPlayerInZone;
    }
    public void ApplyDamageToPlayerByMeleeAttack()
    {
        if (player.TryGetComponent<IDamageable>(out var playerHealth))
        {
            playerHealth.TakeDamage(damage);
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
