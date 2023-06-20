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
        IEnumerator patrolCoroutine;

        const float speed = 2f;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
        }

        public override void Enter()
        {
            agent.locomotionSystem.navMeshAgent.speed = speed;
            agent.locomotionSystem.navMeshAgent.isStopped = false;

            patrolCoroutine = Patrol();
            agent.StartCoroutine(patrolCoroutine);
        }

        public override void Exit()
        {
            agent.StopCoroutine(patrolCoroutine);
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
        }

        IEnumerator Patrol()
        {
            while (true)
            {
                agent.locomotionSystem.navMeshAgent.SetDestination(findNewDestination());
                yield return new WaitForSeconds(10f);
            }
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