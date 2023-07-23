using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class DetectionSystem : MonoBehaviour
    {
        BustAgent agent;

        bool propagated = false;

        void Awake()
        {
            agent = GetComponent<BustAgent>();
            if (agent == null)
                Debug.LogError("DetectionSystem is lacking BustAgent");
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
            Debug.Log("Propagate");
            if (transform.parent == null || transform.parent.tag != "Cluster" || propagated)
                return;
            transform.parent.GetComponent<Enemies.Cluster>().StartEncounter();
            // foreach (var ds in transform.parent.GetComponentsInChildren<DetectionSystem>())
            // {
            //     if ((State)ds.agent.stateMachine.currentState == State.Patrol || (State)ds.agent.stateMachine.currentState == State.Idle)
            //     {
            //         ds.propagated = true;
            //         ds.agent.targetingSystem.hasTarget = true;
            //         print($"Detect from propagate {transform.parent.name}");
            //         ds.agent.stateMachine.ChangeState(State.Pursue);
            //     }
            // }
        }

        public void AlertPlayer()
        {
            GameObject.FindWithTag("Player")?.GetComponent<Character.VisibilitySystem>()?.AlertPlayer();
        }

        public void LosePlayer()
        {
            GameObject.FindWithTag("Player")?.GetComponent<Character.VisibilitySystem>()?.LosePlayer();
        }

        private void OnDisable()
        {
            LosePlayer();
        }
    }
}