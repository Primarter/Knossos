using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Enemy1
{
    public class TargetingSystem : MonoBehaviour
    {
        EnemyAgent agent;
        public Transform target;

        public bool hasTarget;

        void Awake()
        {
            agent = GetComponent<EnemyAgent>();
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