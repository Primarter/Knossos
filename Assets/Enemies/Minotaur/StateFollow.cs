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

        public override void Enter()
        {
            agent.locomotionSystem.navMeshAgent.speed = 5f;
            // agent.locomotionSystem.navMeshAgent.speed = agent.config.defaultSpeed;
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            if (!agent.visionSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.Patrol);
            }
            else
            {
                agent.locomotionSystem.navMeshAgent.destination = player.transform.position;
            }
        }

        public override void Update()
        {
        }
}
}