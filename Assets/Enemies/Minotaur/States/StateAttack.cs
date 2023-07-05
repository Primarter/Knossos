using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos.FSM;

namespace Knossos.Minotaur
{
    public class StateAttack : FSM.State
    {
        MinotaurAgent agent;
        float timer;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.isStopped = true;
            timer = 1.5f;
        }

        public override void Exit(int nextState)
        {
        }

        public override void FixedUpdate()
        {
            agent.attackSystem.Attack();

            timer -= Time.fixedDeltaTime;
            if (timer > 0) return;

            if (agent.visionSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.Follow);
            }
            else
            {
                agent.stateMachine.ChangeState(State.Patrol);
            }
        }

        public override void Update()
        {
            agent.transform.Rotate(new Vector3(0f, 1f, 0f), 360f * Time.deltaTime);
        }
    }
}