using UnityEngine;

public class PatrolState : IEnemyState
{
    private BaseEnemy enemy;
    private float leftLimit, rightLimit, idleDuration;
    private bool isWaiting;
    private float idleTimer;

    public PatrolState(BaseEnemy enemy)
    {
        this.enemy = enemy;
        if (enemy is IPatrolEnemy patrolEnemy)
        {
            leftLimit = patrolEnemy.GetLeftLimit();
            rightLimit = patrolEnemy.GetRightLimit();
            idleDuration = patrolEnemy.GetIdleDuration();
        }
        else
        {
            Debug.LogError("Enemy không implement IPatrolEnemy.");
        }
    }

    public void Enter()
    {
        //Debug.Log("PatrolState Enter");
        isWaiting = false;
        idleTimer = 0f;
        enemy.MoveDirection = Mathf.Sign(enemy.transform.localScale.x);
        SetWalkingAnimation(true);
    }

    public void Execute()
    {
        if (isWaiting)
        {
            idleTimer += Time.fixedDeltaTime;
            if (idleTimer >= idleDuration)
            {
                idleTimer = 0f;
                isWaiting = false;
                enemy.MoveDirection *= -1;
                enemy.FaceDirection(enemy.MoveDirection);
                SetWalkingAnimation(true);
            }
            return;
        }

        enemy.rb.linearVelocity = new Vector2(enemy.GetWalkSpeed() * enemy.MoveDirection, enemy.rb.linearVelocity.y);

        float x = enemy.transform.position.x;
        if ((enemy.MoveDirection < 0 && x <= leftLimit) || (enemy.MoveDirection > 0 && x >= rightLimit))
        {
            enemy.rb.linearVelocity = Vector2.zero;
            isWaiting = true;
            SetWalkingAnimation(false);
        }
    }

    public void Exit()
    {
        //Debug.Log("PatrolState Exit");
        enemy.rb.linearVelocity = Vector2.zero;
        SetWalkingAnimation(false);
    }
    private void SetWalkingAnimation(bool walking)
    {
        enemy.GetEnemyAnimator()?.SetBool(AnimationUtilities.IS_WALKING, walking);
    }
}

