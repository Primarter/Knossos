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

    int damage = 25;

    void Awake()
    {
        agent = GetComponent<MinotaurAgent>();
    }

    void Start()
    {
        attackColliderManager.OnTriggerIn += Attack;
    }

    public void Attack(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // TODO: error here -> redo the ColliderManager
            // other.gameObject.GetComponent<Health>().TakeDamage(damage);
        }

        // GameObject[] players = gameobjectInCollider.Where(i => i.tag == "Player").ToArray();
        // foreach (GameObject player in players)
        // {
        //     player.GetComponent<Health>().TakeDamage(damage);
        // }
    }
}

}
