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
    [SerializeField] ColliderMemory attackColliderMemory;

    int damage = 25;

    void Awake()
    {
        agent = GetComponent<MinotaurAgent>();
    }

    public void Attack()
    {

        // GameObject[] players = gameobjectInCollider.Where(i => i.tag == "Player").ToArray();
        // print(players.Length);

        // foreach (GameObject player in players)
        // {
        //     player.GetComponent<Health>().TakeDamage(damage);
        // }
    }
}

}
