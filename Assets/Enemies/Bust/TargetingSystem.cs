using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class TargetingSystem : MonoBehaviour
    {
        BustAgent agent;
        public Transform target;

        public bool hasTarget;

        void Awake()
        {
            agent = GetComponent<BustAgent>();
        }

        void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            hasTarget = false;
        }

        void FixedUpdate()
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            hasTarget = distanceToTarget < agent.config.triggerAttackDistance;
        }

        void Update()
        {

        }
    }
}