using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StateChargeAttack : FSM.State
    {
        BustAgent agent;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.speed = 0f;
            agent.locomotionSystem.navMeshAgent.isStopped = true;

            Vector3 dir = agent.targetingSystem.target.position - agent.transform.position;
            dir.y = 0f;

            agent.transform.forward = dir.normalized;

            agent.StartCoroutine(attackTimer());
        }

        public override void Exit(int nextState)
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
        }

        IEnumerator attackTimer()
        {
            yield return new WaitForSeconds(agent.config.startAttackCooldown);
            agent.stateMachine.ChangeState(State.Attacking);
        }

    }
}