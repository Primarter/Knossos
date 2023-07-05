using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Enemies
{

[RequireComponent(typeof(Enemy), typeof(OnHitEventSystem))]
public class LifeSystem : MonoBehaviour
{
    private EnemyStats stats;

    float health;

    private void Awake() {
        stats = GetComponent<Enemy>().stats;
        GetComponent<OnHitEventSystem>().onHitCallbacks += TakeDamage;
    }

    private void Start() {
        health = stats.life;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}

}