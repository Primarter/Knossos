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

    void Awake()
    {
        agent = GetComponent<MinotaurAgent>();
    }

    void Start()
    {
    }

    public void Attack()
    {
        foreach (var gameObject in attackColliderManager.getColliding())
        {
            if (gameObject.tag == "Player")
            {
                bool hit = gameObject.GetComponent<Character.Health>().TakeDamage(damage);
            }
        }
    }
}

}
