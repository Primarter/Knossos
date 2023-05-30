using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LocomotionSystem : MonoBehaviour
{
    EnemyAgent agent;
    public NavMeshAgent navMeshAgent;

    void Awake()
    {
        agent = GetComponent<EnemyAgent>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        navMeshAgent.speed = agent.config.defaultSpeed;
    }

    void Update()
    {
        if (!navMeshAgent.enabled) return;
        if (!navMeshAgent.isStopped) return;

        Vector3 dir = new Vector3(transform.forward.x, 0f, transform.forward.z);
        navMeshAgent.Move(dir * navMeshAgent.speed * Time.deltaTime);
    }
}
