using UnityEngine;

public class BrinerofDeathChaseState : IEnemyState
{
    private BrinerofDeathBoss brinerofDeathBoss;
    public BrinerofDeathChaseState(BrinerofDeathBoss brinerofDeathBoss)
    {
        this.brinerofDeathBoss = brinerofDeathBoss;
    }
    public void Enter()
    {
        //Debug.Log("Entering Chase State");
        brinerofDeathBoss.GetBrinerofDeathAnimator().SetBool(AnimationUtilities.IS_WALKING, true);
    }

    public void Execute()
    {
        //Debug.Log("Executing Chase State");
        if (brinerofDeathBoss.CanMeleeAttack())
        {
            brinerofDeathBoss.rb.linearVelocity = Vector2.zero;
            return;
        }
        HandleMovement();
    }
    private void HandleMovement()
    {
        if (brinerofDeathBoss.IsAttacking)
        {
            brinerofDeathBoss.GetBrinerofDeathAnimator()?.SetBool(AnimationUtilities.IS_WALKING, false);
            return;
        }
        float distanceX = brinerofDeathBoss.GetPlayer().position.x - brinerofDeathBoss.transform.position.x;
        float direction = 0f;

        if (Mathf.Abs(distanceX) > 0.1f)
        {
            direction = Mathf.Sign(distanceX);
            brinerofDeathBoss.FaceDirection(direction);
            brinerofDeathBoss.GetBrinerofDeathAnimator()?.SetBool(AnimationUtilities.IS_WALKING, true);
        }
        else
        {
            brinerofDeathBoss.GetBrinerofDeathAnimator()?.SetBool(AnimationUtilities.IS_WALKING, false);
        }

        brinerofDeathBoss.rb.linearVelocity = new Vector2(brinerofDeathBoss.GetWalkSpeed() * direction, brinerofDeathBoss.rb.linearVelocity.y);
    }
    public void Exit()
    {
        //Debug.Log("Exiting Chase State");
        brinerofDeathBoss.GetBrinerofDeathAnimator().SetBool(AnimationUtilities.IS_WALKING, false);
    }
}
