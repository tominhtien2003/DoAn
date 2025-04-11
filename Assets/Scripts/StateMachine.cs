public class StateMachine<T>
{
    private BaseState<T> currentState; private T owner;

    public StateMachine(T owner)
    {
        this.owner = owner;
    }
    public void ChangeState(BaseState<T> newState)
    {
        if (currentState != null && newState != null && currentState.GetType() == newState.GetType())
        {
            return;
        }
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(owner);
    }
    public void Update()
    {
        currentState?.Execute();
    }
}
