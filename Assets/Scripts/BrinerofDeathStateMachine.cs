using UnityEngine;

public class BrinerofDeathStateMachine
{
    private IEnemyState currentState;
    private BrinerofDeathBoss deathBoss;

    public BrinerofDeathStateMachine(BrinerofDeathBoss deathBoss)
    {
        this.deathBoss = deathBoss;
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
    public BrinerofDeathBoss GetBoss() => deathBoss;
}
