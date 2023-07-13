using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knossos.FSM;

namespace Knossos.Minotaur
{
    public class StateFollow : FSM.State
    {
        MinotaurAgent agent;
        GameObject player;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
            player = GameObject.FindWithTag("Player");
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.speed = 5f;
            agent.locomotionSystem.navMeshAgent.isStopped = false;
        }

        public override void Exit(int nextState)
        {
        }

        public override void FixedUpdate()
        {
            if (!agent.visionSystem.hasTarget)
            {
                agent.soundSensorSystem.heardSound(agent.visionSystem.targetPosition);
                agent.stateMachine.ChangeState(State.Alert);
            }
            else
            {
                agent.locomotionSystem.navMeshAgent.destination = player.transform.position;
            }

            if (agent.locomotionSystem.navMeshAgent.remainingDistance < 2.5f)
            {
                agent.stateMachine.ChangeState(State.Attack);
            }

        }

        public override void Update()
        {
        }
}
}