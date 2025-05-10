using System;
using UnityEngine;

public class AttackState : IEnemyState
{
    private BaseEnemy enemy;
    private Transform player;

    private bool isAttacking = false;
    public bool IsAttackingLocal
    {
        get { return isAttacking; }
        set
        {
            isAttacking = value;
            enemy.IsAttacking = isAttacking;
        }
    }

    public AttackState(BaseEnemy enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public void Enter()
    {
        //Debug.Log("AttackState Enter");
        isAttacking = false;
    }

    public void Execute()
    {
        //if (isAttacking)
        //{
        //    enemy.rb.linearVelocity = Vector2.zero;
        //    return;
        //}
        enemy.rb.linearVelocity = Vector2.zero;
        FacePlayer();
        if (enemy.GetCooldownTimer() >= enemy.GetAttackCooldown())
        {
            enemy.ResetCooldown();
            IsAttackingLocal = true;
            enemy.GetEnemyAnimator()?.SetTrigger(AnimationUtilities.ATTACK);
            if (enemy is IRangedEnemy rangedEnemy)
            {
                Transform weaponMountPoint = rangedEnemy.GetWeaponMountPoint();
                if (weaponMountPoint == null)
                {
                    Debug.LogError("Weapon mount point is not assigned for this enemy.");
                    return;
                }

                Transform arrowTransform = ObjectPoolManager.Instance.SpawnObject("Arrow", weaponMountPoint.position, Quaternion.identity);
                if (arrowTransform != null && arrowTransform.TryGetComponent<IWeaponRanged>(out var arrow))
                {
                    float direction = enemy.transform.localScale.x > 0 ? 1f : -1f;
                    arrow.Initialize(weaponMountPoint.position, direction);
                }
            }
            else
            {
                //Debug.Log("Enemy does not implement IRangedEnemy.");
            }
        }
    }

    public void Exit()
    {
        //Debug.Log("AttackState Exit");
        isAttacking = false;
    }

    public void OnAttackAnimationFinished()
    {
        //Debug.Log("Call event when finish attack animation");
        IsAttackingLocal = false;
        enemy.ResetCooldown();
        if (enemy.IsPlayerInAttackZone())
        {
            //Debug.Log("Attack");
            enemy.GetEnemyAnimator()?.SetBool(AnimationUtilities.IS_WALKING, false);
            return;
        }

        if (enemy.PlayerInSight() && !isAttacking)
        {
            //Debug.Log("Chase");
            enemy.GetStateMachine().ChangeState(new ChaseState(enemy));
        }
        else
        {
            //Debug.Log("Patrol");
            enemy.GetStateMachine().ChangeState(new PatrolState(enemy));
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
