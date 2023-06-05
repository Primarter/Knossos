using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{
    EnemyAgent agent;


    public override void Init()
    {
        agent = obj.GetComponent<EnemyAgent>();
    }

    public override void Enter()
    {
        agent.locomotionSystem.navMeshAgent.speed = agent.config.defaultSpeed;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
        if (agent.targetingSystem.hasTarget)
        {
            agent.stateMachine.ChangeState(EnemyState.ChargeAttack);
        }
    }

    public override void Update()
    {
    }
}