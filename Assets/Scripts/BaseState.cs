public abstract class BaseState<T>
{
    protected T owner;
    public virtual void Enter(T owner)
    {
        this.owner = owner;
    }
    public abstract void Execute();
    public abstract void Exit();
}
