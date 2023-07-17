using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    [RequireComponent(typeof(BustAgent), typeof(TargetingSystem))]
    public class DetectionSystem : MonoBehaviour
    {
        BustAgent agent;

        bool propagated = false;

        void Awake()
        {
            agent = GetComponent<BustAgent>();
        }

        private void Update()
        {
            if (agent.targetingSystem.hasTarget == false)
            {
                propagated = false;
            }
        }

        public void PropagateDetection()
        {
            if (transform.parent == null || transform.parent.tag != "Cluster" || propagated)
                return;
            foreach (var ds in transform.parent.GetComponentsInChildren<DetectionSystem>())
            {
                if ((State)ds.agent.stateMachine.currentState == State.Patrol)
                {
                    ds.propagated = true;
                    ds.agent.targetingSystem.hasTarget = true;
                    ds.agent.stateMachine.ChangeState(State.Pursue);
                }
            }
        }
    }
}