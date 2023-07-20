using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class TriggerZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (transform.parent == null || transform.parent.tag != "Cluster" || other.tag != "Player")
                return;
            transform.parent.GetComponent<Enemies.Cluster>()?.StartEncounter();
            foreach (var agent in transform.parent.GetComponentsInChildren<BustAgent>())
            {
                agent.targetingSystem.hasTarget = true;
                agent.stateMachine.ChangeState(State.Pursue);
            }
        }
    }

}