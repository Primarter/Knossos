using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : State
{
    public int GetId()
    {
        return (int)EnemyState.Idle;
    }

    void State.Enter(StateMachineAgent agent)
    {
        Debug.Log("Enter Idle State");
    }

    void State.Exit(StateMachineAgent agent)
    {
        Debug.Log("Exit Idle State");
    }

    void State.Update(StateMachineAgent agent)
    {
        Debug.Log("Idle update");
        // EnemyAgent a = (EnemyAgent)agent;
        agent.transform.Rotate(new Vector3(0f, 2f, 0f));

    }
}
