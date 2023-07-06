using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StateEndlag : FSM.State
    {
        BustAgent agent;
        Quaternion originalRotation;
        Coroutine cooldown;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.enabled = false;
            // agent.locomotionSystem.navMeshAgent.speed = 0f;
            // agent.locomotionSystem.navMeshAgent.isStopped = true;

            originalRotation = agent.transform.rotation;
            agent.transform.rotation *= Quaternion.AngleAxis(90f, new Vector3(1f, 0f, 0f));

            cooldown = agent.StartCoroutine(CooldownTimer());
        }

        public override void Exit(int nextState)
        {
            agent.transform.rotation = originalRotation;
            agent.locomotionSystem.navMeshAgent.enabled = true;
            // agent.locomotionSystem.navMeshAgent.isStopped = false;
            agent.StopCoroutine(cooldown);
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            // agent.transform.Rotate(new Vector3(0f, 360f, 0f) * Time.deltaTime);
        }

        IEnumerator CooldownTimer()
        {
            yield return new WaitForSeconds(agent.config.attackEndlag);
            // agent.stateMachine.ChangeState(EnemyState.Idle);
            agent.stateMachine.ChangeState(State.Pursue);
        }
    }
}