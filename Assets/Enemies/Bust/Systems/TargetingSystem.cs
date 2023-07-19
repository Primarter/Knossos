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
                usesTriggerZones = transform.parent.GetComponentsInChildren<TriggerZone>().Length > 0;
        }

        void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            distanceToTarget = Vector3.Distance(transform.position, target.position);
        }

        void FixedUpdate()
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget < agent.config.detectionRange && !usesTriggerZones)
                hasTarget = true;
            if (distanceToTarget > agent.config.maxDetectionRange)
                hasTarget = false;
        }

        void Update()
        {

        }
    }
}