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
                    hasHit = gameObject.GetComponent<Character.Health>().TakeDamage(damage);
                }
            }
        }
    }
}

}
