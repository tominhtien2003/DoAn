using UnityEngine;

public class BrinerofDeathMeleeAttackState : IEnemyState
{
    private BrinerofDeathBoss brinerofDeathBoss;
    private bool isAttacking = false;
    public bool IsAttacking
    {
        get { return isAttacking; }
        set
        {
            isAttacking = value;
            brinerofDeathBoss.IsAttacking = isAttacking;
        }
    }
    public BrinerofDeathMeleeAttackState(BrinerofDeathBoss brinerofDeathBoss)
    {
        this.brinerofDeathBoss = brinerofDeathBoss;
    }
    public void Enter()
    {
        //Debug.Log("Entering Melee Attack State");
        IsAttacking = false;
    }

    public void Execute()
    {
        //Debug.Log("Executing Melee Attack State");
        brinerofDeathBoss.rb.linearVelocity = Vector2.zero;
        if (IsAttacking)
        {
            return;
        }
        FacePlayer();
        if (brinerofDeathBoss.GetCooldownTimer() >= brinerofDeathBoss.GetAttackCooldown())
        {
            brinerofDeathBoss.ResetCooldown();
            IsAttacking = true;
            brinerofDeathBoss.GetBrinerofDeathAnimator()?.SetTrigger(AnimationUtilities.ATTACK_MELEE);
        }
    }

    public void Exit()
    {
        //Debug.Log("Exiting Melee Attack State");
        isAttacking = false;
    }
    public void OnMeleeAttackAnimationFinished()
    {
        //Debug.Log("Call event when finish attack animation");
        IsAttacking = false;
        brinerofDeathBoss.ResetCooldown();
        if (brinerofDeathBoss.IsPlayerInMeleeAttackZone())
        {
            //Debug.Log("Attack");
            return;
        }

        if (brinerofDeathBoss.PlayerInMeleeRange() && !IsAttacking)
        {
            //Debug.Log("Chase");
            brinerofDeathBoss.GetStateMachine().ChangeState(new BrinerofDeathChaseState(brinerofDeathBoss));
        }
        else
        {
            
        }
    }
    private void FacePlayer()
    {
        float diff = brinerofDeathBoss.GetPlayer().position.x - brinerofDeathBoss.transform.position.x;

        if (Mathf.Abs(diff) > 0.1f)
        {
            float direction = Mathf.Sign(diff);
            brinerofDeathBoss.FaceDirection(direction);
        }
    }
}
