using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Enemies
{

[RequireComponent(typeof(OnHitEventSystem))]
public class LifeSystem : MonoBehaviour
{
    [SerializeField]
    int startHealth = 3;

    int health;

    private void Awake() {
        GetComponent<OnHitEventSystem>().onHitCallbacks += TakeDamage;
    }

    private void Start() {
        health = startHealth;
    }

    public void TakeDamage(OnHitEventSystem.HitInfo hitInfo)
    {
        health -= hitInfo.damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}

}