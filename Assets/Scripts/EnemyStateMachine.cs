using UnityEngine;

public class EnemyStateMachine
{
    private IEnemyState currentState;
    private BaseEnemy enemy;

    public EnemyStateMachine(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null && currentState.GetType() == newState.GetType()) return;
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void ExitCurrentState()
    {
        currentState?.Exit();
        currentState = null;
    }
    public IEnemyState GetCurrentState() => currentState;
    public BaseEnemy GetBaseEnemy() => enemy;
}
