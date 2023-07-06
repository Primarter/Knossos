using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StateAttacking : FSM.State
    {
        BustAgent agent;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.speed = agent.config.attackSpeed;
            agent.StartCoroutine(AttackTimer());
        }

        public override void Exit(int nextState)
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            // agent.transform.Rotate(new Vector3(0f, 90f, 0f) * Time.deltaTime);
        }

        IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(agent.config.attackDuration);
            agent.stateMachine.ChangeState(State.Endlag);
        }
    }
}