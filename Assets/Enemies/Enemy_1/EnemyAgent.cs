using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : StateMachineAgent
{
    [SerializeField] EnemyState currentState;

    void Start()
    {
        stateMachine = new StateMachine(this, typeof(EnemyState));

        stateMachine.RegisterState(new EnemyStateIdle());

        // stateMachine.ChangeState(initialState);
        stateMachine.ChangeState((int)EnemyState.Idle);
    }


    void Update()
    {
        stateMachine.Update();
        currentState = (EnemyState)stateMachine.currentState;
    }
}
