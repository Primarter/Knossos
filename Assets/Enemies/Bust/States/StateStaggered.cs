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
        Coroutine knockback;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            this.previousState = previousState;

            agent.transform.forward = -1 * agent.staggerSystem.lastHit.hitDirection.normalized;
            // agent.locomotionSystem.navMeshAgent.velocity = agent.staggerSystem.lastHit.hitDirection.normalized * agent.config.defaultSpeed * agent.staggerSystem.lastHit.knockBackStrength;
            originalRotation = agent.transform.rotation;

            knockback = agent.StartCoroutine(KnockBackTimer());
            staggered = agent.StartCoroutine(StaggeredTimer());
        }

        public override void Exit(int nextState)
        {
            agent.transform.rotation = originalRotation;
            agent.locomotionSystem.navMeshAgent.velocity = new Vector3();
            agent.locomotionSystem.navMeshAgent.isStopped = false;
            if (staggered != null)
                agent.StopCoroutine(staggered);
            if (knockback != null)
                agent.StopCoroutine(knockback);
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            agent.transform.rotation = originalRotation;
        }

        IEnumerator KnockBackTimer()
        {
            var start = Time.time;

            while (Time.time < start + agent.staggerSystem.lastHit.knockBackDuration)
            {
                float progress = Easing.easeOutExpo(((Time.time - start) / agent.staggerSystem.lastHit.knockBackDuration));
                agent.locomotionSystem.navMeshAgent.velocity = agent.staggerSystem.lastHit.hitDirection.normalized * agent.config.defaultSpeed * agent.staggerSystem.lastHit.knockBackStrength * Time.deltaTime * progress;
                yield return null;
            }
            agent.locomotionSystem.navMeshAgent.velocity = new Vector3();
            agent.locomotionSystem.navMeshAgent.isStopped = true;
            // TODO: SET DESTINATION AND navmeshagent.updateRotation = false; and direction to smtg behind it
        }

        IEnumerator StaggeredTimer()
        {
            yield return new WaitForSeconds(agent.staggerSystem.lastHit.staggerDuration);
            agent.stateMachine.ChangeState(State.Pursue);
        }
    }
}