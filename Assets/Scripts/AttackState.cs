using System;
using UnityEngine;

public class AttackState : IEnemyState
{
    private KnightEnemy enemy;
    private Transform player;

    private float attackCooldown = 2f;
    private float cooldownTimer = 0f;

    private bool isAttacking = false;
    public bool IsAttacking
    {
        get { return isAttacking; }
        set
        {
            isAttacking = value;
            enemy.IsAttacking = isAttacking;
        }
    }

    public AttackState(KnightEnemy enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public void Enter()
    {
        isAttacking = false;
        cooldownTimer = attackCooldown; // Đánh ngay lần đầu
    }

    public void Execute()
    {
        if (isAttacking)
        {
            enemy.rb.linearVelocity = Vector2.zero;
            return;
        }

        cooldownTimer += Time.fixedDeltaTime;
        enemy.rb.linearVelocity = Vector2.zero;
        FacePlayer();

        if (enemy.CooldownTimer >= enemy.AttackCooldown)
        {
            enemy.CooldownTimer = 0f; // reset cooldown khi bắt đầu đánh
            IsAttacking = true;
            enemy.GetEnemyAnimator()?.SetTrigger(AnimationUtilities.ATTACK);
        }
    }

    public void Exit()
    {
        isAttacking = false;
        cooldownTimer = 0f;
    }

    public void OnAttackAnimationFinished()
    {
        isAttacking = false;

        if (enemy.IsPlayerInAttackZone())
        {
            // Player vẫn trong vùng tấn công → tiếp tục attack
            return;
        }

        if (enemy.PlayerInSight() && !isAttacking)
        {
            // Player đã rời vùng attack, nhưng vẫn trong khu vực → đuổi theo
            enemy.ChangeState(new ChaseState(enemy));
        }
        else
        {
            // Player biến khỏi khu vực → quay lại tuần tra
            enemy.ChangeState(new PatrolState(enemy, enemy.leftLimitPos, enemy.rightLimitPos, enemy.patrolIdleTime));
        }
    }

    private void FacePlayer()
    {
        float diff = player.position.x - enemy.transform.position.x;

        if (Mathf.Abs(diff) > 0.1f) 
        {
            float direction = Mathf.Sign(diff);
            enemy.MoveDirection = direction;
            enemy.FaceDirection(direction);
        }
    }
}
