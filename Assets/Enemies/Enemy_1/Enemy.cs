using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;

    public float triggerAttackDistance = 5f;
    public float startAttackCooldown = 0.3f;
    public float attackTime = 0.3f;
    public float endAttackCooldown = 0.3f;
    public float nextAttackCooldown = 2f;

    public bool isAttacking;
    public bool canAttack;

    float attackSpeed = 12f;
    // float defaultSpeed = 1f;
    // float speed;

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

        yield return new WaitForSeconds(startAttackCooldown);

        // state: attack
        isAttacking = true;
        // set speed to 10
        // speed = attackSpeed;
        yield return new WaitForSeconds(attackTime);


        // state endOfAttack
        // speed = 0f;
        yield return new WaitForSeconds(endAttackCooldown);

        // state cooldownForNextAttack
        agent.isStopped = false;
        isAttacking = false;
        yield return new WaitForSeconds(nextAttackCooldown);

        // state: after attack cooldown
        // go back to initial state
        canAttack = true;
    }

    void FixedUpdate()
    {
        if (canAttack)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < triggerAttackDistance)
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
            agent.Move(dir * attackSpeed * Time.deltaTime);
        }
    }
}
