using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace Knossos.Minotaur
{

public class AttackSystem : MonoBehaviour
{
    MinotaurAgent agent;
    [SerializeField] ColliderManager attackColliderManager;
    [SerializeField] int damage = 50;

    bool _hitting = false;
    public bool hitting
    {
        get => _hitting;
        set
        {
            hasHit = false;
            _hitting = value;
        }
    }

    bool hasHit = false;

    void Awake()
    {
        agent = GetComponent<MinotaurAgent>();
    }

    private void Update()
    {
        if (hitting && !hasHit)
        {
            foreach (var gameObject in attackColliderManager.getColliding())
            {
                if (gameObject.tag == "Player")
                {
                    Vector3 playerPos = gameObject.transform.position;
                    Character.Health health = gameObject.GetComponent<Character.Health>();
                    hasHit = health.TakeDamage(damage, out bool died);
                    if (died)
                    {
                        agent.visionSystem.hasTarget = false;
                        agent.stateMachine.ChangeState(State.Patrol);
                    }
                }
            }
        }
    }
}

}
