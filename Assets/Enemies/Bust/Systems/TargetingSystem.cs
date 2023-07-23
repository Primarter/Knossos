using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class TargetingSystem : MonoBehaviour
    {
        BustAgent agent;
        public Transform target;

        private bool _hasTarget = false;
        public bool hasTarget
        {
            get => _hasTarget;
            set
            {
                if (value && !_hasTarget)
                    agent.detectionSystem.AlertPlayer();
                else if (!value && _hasTarget)
                    agent.detectionSystem.LosePlayer();
                _hasTarget = value;
            }
        }

        public bool isInRange { get => distanceToTarget <= agent.config.attackRange; }

        float distanceToTarget;
        bool usesTriggerZones = false;

        void Awake()
        {
            agent = GetComponent<BustAgent>();
            if (agent == null)
                Debug.LogError("TargetingSystem is lacking BustAgent");
            if (transform.parent != null && transform.parent.tag == "Cluster")
                usesTriggerZones = transform.parent.GetComponentsInChildren<Enemies.TriggerZone>().Length > 0;
        }

        void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            distanceToTarget = Vector3.Distance(transform.position, target.position);
        }

        void FixedUpdate()
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!hasTarget && !usesTriggerZones && distanceToTarget < agent.config.detectionRange && CheckLineOfSight())
                hasTarget = true;
            // if (distanceToTarget > agent.config.maxDetectionRange)
            //     hasTarget = false;
        }

        bool CheckLineOfSight()
        {
            RaycastHit hit;
            Vector3 targetPos = target.position + new Vector3(0f, 1.0f, 0f);
            return Physics.Raycast(transform.position, targetPos - transform.position, out hit)
                && hit.transform.tag == "Player";
        }

        void OnDrawGizmosSelected()
        {
            var ag = GetComponent<BustAgent>();
            if (ag.config != null)
                Gizmos.DrawWireSphere(transform.position, ag.config.detectionRange);
        }
    }
}