public interface State
{
    // StateId GetId();
    int GetId();
    void Enter(StateMachineAgent agent);
    void Update(StateMachineAgent agent);
    void Exit(StateMachineAgent agent);
}
