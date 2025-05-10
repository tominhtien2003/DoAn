using UnityEngine;

public class BrinerofDeathStateMachine
{
    private BrinerofDeathState currentState;
    private BrinerofDeathBoss brinerofDeathBoss;

    public BrinerofDeathStateMachine(BrinerofDeathBoss brinerofDeathBoss)
    {
        this.brinerofDeathBoss = brinerofDeathBoss;
    }

    public void ChangeState(BrinerofDeathState newState)
    {
        if (currentState != null && currentState.GetType() == newState.GetType()) return;
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
    public BrinerofDeathState GetCurrentState()
    {
        return currentState;
    }
}
