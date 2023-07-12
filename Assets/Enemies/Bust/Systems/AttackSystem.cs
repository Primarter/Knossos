using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knossos.Bust
{

public class AttackSystem : MonoBehaviour
{
    [SerializeField]
    int damage = 10;

    UnityEvent onHitPlayerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Character.Health>().TakeDamage(damage);
        }

        var bell = other.GetComponent<Bell.AttractMinotaur>();
        if (bell != null)
        {
            bell.RingBell();
            bell.GetComponent<Enemies.OnHitEventSystem>().TakeDamage();
        }
    }
}

}