using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos.FSM;

namespace Knossos.Minotaur
{
    public class StatePatrol : FSM.State
    {
        MinotaurAgent agent;

        GameObject targetWaypoint;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
        }

        public override void Enter()
        {
            agent.locomotionSystem.navMeshAgent.speed = 3f;
            agent.locomotionSystem.navMeshAgent.isStopped = false;

            // patrolCoroutine = Patrol();
            // agent.StartCoroutine(patrolCoroutine);
        }

        public override void Exit()
        {
            // agent.StopCoroutine(patrolCoroutine);
        }

        public override void FixedUpdate()
        {
            if (agent.visionSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.Follow);
            }
        }

        public override void Update()
        {
        }

        Vector3 findNewDestination()
        {
            float minRange = 20f;
            float maxRange = 70f;
            Vector3 point = Random.onUnitSphere * Random.Range(minRange, maxRange);

            NavMeshHit hit;
            NavMesh.SamplePosition(agent.transform.position + point, out hit, 30f, 1);

            return hit.position;
        }
    }
}