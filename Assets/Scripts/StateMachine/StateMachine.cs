using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State[] states;
    public StateMachineAgent agent;
    public int currentState;

    public StateMachine(StateMachineAgent agent, System.Type enumStateType)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(enumStateType).Length;
        // Debug.Log(("numStates", numStates));
        states = new State[numStates];
    }

    public void RegisterState(State state)
    {
        int index = (int)(object)state.GetId();
        states[index] = state;
    }

    public State GetState(int stateId)
    {
        int index = (int) stateId;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(int newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}