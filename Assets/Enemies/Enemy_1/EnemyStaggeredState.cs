using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStagerredState : State
{
    EnemyAgent agent;
    Quaternion originalRotation;

    public override void Init()
    {
        agent = obj.GetComponent<EnemyAgent>();
    }

    public override void Enter()
    {
        agent.locomotionSystem.navMeshAgent.enabled = false;
        // agent.locomotionSystem.navMeshAgent.speed = 0f;
        // agent.locomotionSystem.navMeshAgent.isStopped = true;

        originalRotation = agent.transform.rotation;
        agent.transform.rotation *= Quaternion.AngleAxis(90f, new Vector3(1f, 0f, 0f));

        agent.StartCoroutine(StaggeredTimer());
    }

    public override void Exit()
    {
        agent.transform.rotation = originalRotation;
        agent.locomotionSystem.navMeshAgent.enabled = true;
        // agent.locomotionSystem.navMeshAgent.isStopped = false;
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        // agent.transform.Rotate(new Vector3(0f, 360f, 0f) * Time.deltaTime);
    }

    IEnumerator StaggeredTimer()
    {
        yield return new WaitForSeconds(agent.config.endAttackCooldown);
        agent.stateMachine.ChangeState(EnemyState.Idle);
    }
}
