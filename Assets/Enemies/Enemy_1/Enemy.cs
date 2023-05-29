using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;

    public EnemyConfig config;

    public bool isAttacking;
    public bool canAttack;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // speed = defaultSpeed;
        canAttack = true;
        isAttacking = false;
    }

    IEnumerator Attack()
    {
        canAttack = false;

        Vector3 dir = target.position - transform.position;
        transform.forward = new Vector3(dir.x, 0f, dir.z);
        // speed = 0f;
        // agent.isStopped = true;

        yield return new WaitForSeconds(config.startAttackCooldown);

        // state: attack
        isAttacking = true;
        // set speed to 10
        // speed = attackSpeed;
        yield return new WaitForSeconds(config.attackDuration);

        // state endOfAttack
        // speed = 0f;
        yield return new WaitForSeconds(config.endAttackCooldown);

        // state cooldownForNextAttack
        agent.isStopped = false;
        isAttacking = false;
        yield return new WaitForSeconds(config.nextAttackCooldown);

        // state: after attack cooldown
        // go back to initial state
        canAttack = true;
    }

    void FixedUpdate()
    {
        if (canAttack)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < config.triggerAttackDistance)
            {
                print("YOLODWO");
                StartCoroutine(Attack());
            }
        }

    }

    void Update()
    {
        if (isAttacking)
        {
            // transform.
            Vector3 dir = new Vector3(transform.forward.x, 0f, transform.forward.z);
            agent.Move(dir * config.attackSpeed * Time.deltaTime);
        }
    }
}
