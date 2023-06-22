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

        Waypoint targetWaypoint;

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
                Waypoint waypoint = agent.pathSystem.getClosestWaypoint();
                targetWaypoint = waypoint;

                agent.locomotionSystem.navMeshAgent.destination = targetWaypoint.obj.transform.position;
            }
            else
            {
                Vector3 targetWaypoint2D = new Vector3(targetWaypoint.obj.transform.position.x, 0f, targetWaypoint.obj.transform.position.z);
                Vector3 agentPosition2D = new Vector3(agent.transform.position.x, 0f, agent.transform.position.z);

                if (Vector3.Distance(targetWaypoint2D, agentPosition2D) < 1f)
                {
                    setTarget(findNewTarget());
                }
            }
        }

        public override void Update()
        {
        }

        void setTarget(Waypoint target)
        {
            targetWaypoint = target;
            agent.locomotionSystem.navMeshAgent.destination = target.obj.transform.position;
        }

        Waypoint findNewTarget()
        {
            Waypoint randomWaypoint = targetWaypoint.neighbours[Random.Range(0, targetWaypoint.neighbours.Length)];
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