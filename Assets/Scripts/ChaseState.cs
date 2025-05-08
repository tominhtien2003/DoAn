using UnityEngine;
public class ChaseState : IEnemyState
{
    private BaseEnemy enemy;
    private Transform player;

    public ChaseState(BaseEnemy enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public void Enter()
    {
        //Debug.Log("ChaseState Enter");
        enemy.GetEnemyAnimator()?.SetBool(AnimationUtilities.IS_WALKING, true);
    }

    public void Execute()
    {
        if (enemy.IsAttacking)
        {
            enemy.GetEnemyAnimator()?.SetBool(AnimationUtilities.IS_WALKING, false);
            return;
        }
        if (!enemy.PlayerInSight())
        {
            enemy.GetStateMachine().ChangeState(new PatrolState(enemy));
            return;
        }
        float distance = player.position.x - enemy.transform.position.x;
        float dir = 0f;
        if (Mathf.Abs(distance) > 0.1f)
        {
            dir = Mathf.Sign(distance);
            enemy.MoveDirection = dir;
            enemy.FaceDirection(dir);
            enemy.GetEnemyAnimator()?.SetBool(AnimationUtilities.IS_WALKING, true);
        }
        else
        {
            enemy.GetEnemyAnimator()?.SetBool(AnimationUtilities.IS_WALKING, false);
        }
        enemy.rb.linearVelocity = new Vector2(enemy.GetWalkSpeed() * dir, enemy.rb.linearVelocity.y);
    }

    public void Exit()
    {
        //Debug.Log("ChaseState Exit");   
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.GetEnemyAnimator()?.SetBool(AnimationUtilities.IS_WALKING, false);
    }
}
