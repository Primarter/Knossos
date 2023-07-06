using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StateStagered : FSM.State
    {
        BustAgent agent;
        Quaternion originalRotation;
        int previousState;
        Coroutine staggered;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            this.previousState = previousState;
            agent.locomotionSystem.navMeshAgent.enabled = false;

            originalRotation = agent.transform.rotation;
            agent.transform.rotation *= Quaternion.AngleAxis(90f, new Vector3(-1f, 0f, 0f));

            staggered = agent.StartCoroutine(StaggeredTimer());
        }

        public override void Exit(int nextState)
        {
            agent.transform.rotation = originalRotation;
            agent.locomotionSystem.navMeshAgent.enabled = true;
            agent.StopCoroutine(staggered);
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
        }

        IEnumerator StaggeredTimer()
        {
            yield return new WaitForSeconds(agent.config.staggerDuration);
            agent.stateMachine.ChangeState(State.Pursue);
        }
    }
}