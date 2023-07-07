using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knossos.Bust
{

public class AttackSystem : MonoBehaviour
{
    BustAgent agent;

    UnityEvent onHitPlayerEvent;

    void Awake()
    {
        agent = GetComponent<BustAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Hit Player");
        }

        var bell = other.GetComponent<Bell.AttractMinotaur>();
        if (bell != null)
        {
            bell.RingBell();
            bell.GetComponent<Enemies.OnHitEventSystem>().TakeDamage(0);
        }
    }
}

}