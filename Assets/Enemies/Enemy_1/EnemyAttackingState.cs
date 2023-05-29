using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : State
{
    EnemyAgent agent;

    public override void Init()
    {
        agent = obj.GetComponent<EnemyAgent>();
    }

    public override void Enter()
    {
        agent.locomotionSystem.navMeshAgent.speed = agent.config.attackSpeed;
        agent.StartCoroutine(AttackTimer());
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        // agent.transform.Rotate(new Vector3(0f, 90f, 0f) * Time.deltaTime);
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(agent.config.attackDuration);
        agent.stateMachine.ChangeState(EnemyState.Stagerred);
    }
}
