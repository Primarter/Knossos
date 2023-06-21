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

            targetWaypoint = null;
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            if (agent.visionSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.Follow);
            }

            if (targetWaypoint == null)
            {
                GameObject waypoint = agent.pathSystem.getClosestWaypoint();
                targetWaypoint = waypoint;

                agent.locomotionSystem.navMeshAgent.destination = targetWaypoint.transform.position;
            }
            else
            {
                Vector3 targetWaypoint2D = new Vector3(targetWaypoint.transform.position.x, 0f, targetWaypoint.transform.position.z);
                Vector3 agentPosition2D = new Vector3(agent.transform.position.x, 0f, agent.transform.position.z);

                if (Vector3.Distance(targetWaypoint2D, agentPosition2D) < 1f)
                {
                    GameObject newTarget = findNewTarget();
                    setTarget(newTarget);
                }
            }
        }

        public override void Update()
        {
        }

        void setTarget(GameObject target)
        {
            targetWaypoint = target;
            agent.locomotionSystem.navMeshAgent.destination = target.transform.position;
        }

        GameObject findNewTarget()
        {
            GameObject[] linkedWaypoints = agent.pathSystem.getLinkedWaypoints(targetWaypoint);
            GameObject randomWaypoint = linkedWaypoints[Random.Range(0, linkedWaypoints.Length)];

            return randomWaypoint;

            // float minRange = 20f;
            // float maxRange = 70f;
            // Vector3 point = Random.onUnitSphere * Random.Range(minRange, maxRange);

            // NavMeshHit hit;
            // NavMesh.SamplePosition(agent.transform.position + point, out hit, 30f, 1);

            // return hit.position;
        }
    }
}