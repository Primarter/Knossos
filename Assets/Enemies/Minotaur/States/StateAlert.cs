using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos.FSM;

namespace Knossos.Minotaur
{
    public class StateAlert : FSM.State
    {
        MinotaurAgent agent;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
        }

        public override void Enter()
        {
            agent.soundSensorSystem.heardSuspiciousSound = false;

            agent.locomotionSystem.navMeshAgent.isStopped = false;
            agent.locomotionSystem.navMeshAgent.destination = agent.soundSensorSystem.suspiciousSoundPosition;
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            if (agent.visionSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.Follow);
            }

            if (agent.locomotionSystem.navMeshAgent.remainingDistance < 1f)
            {
                agent.stateMachine.ChangeState(State.Patrol);
            }
        }

        public override void Update()
        {
        }
    }
}