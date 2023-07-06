using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StatePursue : FSM.State
    {
        BustAgent agent;
        bool isCoolingDown = false;
        Coroutine cooldown;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.speed = agent.config.defaultSpeed;
            if ((State)previousState == State.Endlag)
                cooldown = agent.StartCoroutine(Cooldown());

            if ((State)previousState == State.Patrol)
            {
                agent.GetComponent<DetectionSystem>()?.PropagateDetection();
            }
        }

        public override void Exit(int nextState)
        {
            if (cooldown != null)
                agent.StopCoroutine(cooldown);
        }

        public override void FixedUpdate()
        {
            if (agent.targetingSystem.hasTarget)
            {
                agent.locomotionSystem.navMeshAgent.SetDestination(agent.targetingSystem.target.position);
                if (agent.targetingSystem.isInRange)
                {
                    agent.locomotionSystem.navMeshAgent.isStopped = true;
                    if (!isCoolingDown)
                        agent.stateMachine.ChangeState(State.ChargeAttack);
                }
                else
                    agent.locomotionSystem.navMeshAgent.isStopped = false;
            }
            else
                agent.stateMachine.ChangeState(State.Patrol);
        }

        public override void Update()
        {
        }

        IEnumerator Cooldown()
        {
            isCoolingDown = true;
            yield return new WaitForSeconds(agent.config.attackCooldown);
            isCoolingDown = false;
        }
    }
}