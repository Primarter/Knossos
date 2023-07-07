using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    [RequireComponent(typeof(BustAgent))]
    public class TargetingSystem : MonoBehaviour
    {
        BustAgent agent;
        public Transform target;

        [HideInInspector]
        public bool hasTarget = false;
        public bool isInRange { get => distanceToTarget <= agent.config.attackRange; }

        float distanceToTarget;

        void Awake()
        {
            agent = GetComponent<BustAgent>();
        }

        void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            distanceToTarget = Vector3.Distance(transform.position, target.position);
        }

        void FixedUpdate()
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget < agent.config.detectionRange)
                hasTarget = true;
            if (distanceToTarget > agent.config.maxDetectionRange)
                hasTarget = false;
        }

        void Update()
        {

        }
    }
}