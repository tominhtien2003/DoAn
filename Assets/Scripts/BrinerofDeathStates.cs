using UnityEngine;

public abstract class BrinerofDeathState
{
    protected BrinerofDeathBoss boss;

    public BrinerofDeathState(BrinerofDeathBoss boss)
    {
        this.boss = boss;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
public class BrinerofDeathChaseState : BrinerofDeathState
{
    public BrinerofDeathChaseState(BrinerofDeathBoss boss) : base(boss) { }

    public override void Enter()
    {
        boss.GetBrinerofDeathAnimator().SetBool(AnimationUtilities.IS_WALKING, true);
    }

    public override void Update()
    {
        Vector2 direction = (boss.GetPlayer().position - boss.transform.position).normalized;
        boss.transform.position += new Vector3(direction.x, 0, 0) * boss.GetMoveSpeed() * Time.deltaTime;
        boss.FaceDirection(direction.x);
    }
    public override void Exit()
    {
        boss.GetBrinerofDeathAnimator().SetBool(AnimationUtilities.IS_WALKING, false);
    }
}

public class BrinerofDeathMeleeAttackState : BrinerofDeathState
{
    public BrinerofDeathMeleeAttackState(BrinerofDeathBoss boss) : base(boss) { }

    public override void Enter()
    {
        boss.IsAttacking = true;
        float directionToPlayer = Mathf.Sign(boss.GetPlayer().position.x - boss.transform.position.x);
        boss.FaceDirection(directionToPlayer);
    }

    public override void Update()
    {
        boss.rb.linearVelocity = Vector2.zero;
        if (boss.CanMeleeAttack)
        {
            boss.ResetMeleeCooldown();
            boss.GetBrinerofDeathAnimator().SetTrigger(AnimationUtilities.ATTACK_MELEE);
        }
    }

    public override void Exit()
    {
        //Debug.Log("Melee Attack Exit");
    }
}

public class BrinerofDeathDarkOrbState : BrinerofDeathState
{
    public BrinerofDeathDarkOrbState(BrinerofDeathBoss boss) : base(boss) { }

    public override void Enter()
    {
        boss.IsAttacking = true;
        float directionToPlayer = Mathf.Sign(boss.GetPlayer().position.x - boss.transform.position.x);
        boss.FaceDirection(directionToPlayer);
    }

    public override void Update()
    {
        boss.rb.linearVelocity = Vector2.zero;
        if (boss.CanDarkOrbAttack && !boss.IsDroppingDarkOrb)
        {
            boss.IsDroppingDarkOrb = true;
            boss.SpawnDarkOrbs();
            boss.GetBrinerofDeathAnimator().SetTrigger(AnimationUtilities.DARK_ORB);
        }
    }

    public override void Exit()
    {
        //Debug.Log("Dark Orb Exit");
    }
} 