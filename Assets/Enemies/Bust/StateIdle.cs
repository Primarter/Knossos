using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StateIdle : FSM.State
    {
        BustAgent agent;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.speed = agent.config.defaultSpeed;
        }

        public override void Exit(int nextState)
        {
        }

        public override void FixedUpdate()
        {
            if (agent.targetingSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.ChargeAttack);
            }
        }

        public override void Update()
        {
        }
}
}