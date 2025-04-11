using UnityEngine;
public class ChaseState : IEnemyState
{
    private KnightEnemy enemy;
    private Transform player;

    public ChaseState(KnightEnemy enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public void Enter()
    {
        enemy.GetEnemyAnimator()?.SetBool(AnimationUtilities.IS_WALKING, true);
    }

    public void Execute()
    {
        if (!enemy.PlayerInSight())
        {
            enemy.ChangeState(new PatrolState(enemy, enemy.leftLimitPos, enemy.rightLimitPos, enemy.patrolIdleTime));
            return;
        }

        float dir = Mathf.Sign(player.position.x - enemy.transform.position.x);
        enemy.MoveDirection = dir;
        enemy.FaceDirection(dir);

        enemy.rb.linearVelocity = new Vector2(enemy.GetWalkSpeed() * dir, enemy.rb.linearVelocity.y);
    }

    public void Exit()
    {
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.GetEnemyAnimator()?.SetBool(AnimationUtilities.IS_WALKING, false);
    }
}
