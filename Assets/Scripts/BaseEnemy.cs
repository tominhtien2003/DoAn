using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected Animator enemyAnimator;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float damage = 10f;

    public Rigidbody2D rb { get; set; }
    public float MoveDirection { get; set; }
    public bool IsHurting { get; set; }

    public float GetWalkSpeed() => walkSpeed;
    public Animator GetEnemyAnimator() => enemyAnimator;

    public virtual void FaceDirection(float dir)
    {
        if (Mathf.Approximately(dir, 0f)) return;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dir);
        transform.localScale = scale;
    }
    protected BaseEnemy GetBaseEnemy()
    {
        return this;
    }
    public abstract void ApplyDamageToPlayer();
    public abstract EnemyType EnemyType { get; }
    public abstract Transform GetPlayer();
    public abstract bool PlayerInSight();
    public abstract bool IsPlayerInAttackZone();
    public abstract float GetAttackCooldown();
    public abstract float GetCooldownTimer();
    public abstract void ResetCooldown();
    public abstract void OnAttackAnimationFinished();
    public abstract EnemyStateMachine GetStateMachine();
    public abstract bool IsAttacking { get; set; }
}
