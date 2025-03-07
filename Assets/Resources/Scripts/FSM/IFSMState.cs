
public interface IFSMState<T>
{
    void Enter(T e);    // Start State

    void Execute(T e);  // Stay State

    void Exit(T e);     // Exit State
}