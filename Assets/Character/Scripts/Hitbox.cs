using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Hitbox : MonoBehaviour
{
    [SerializeField]
    AnimationController animationController;
    public int hitIdx = 0;

    BoxCollider collider;
    List<EnemyLife> enemies = new();
    List<EnemyLife> hitEnemies = new();
    bool hitting = false;
    int damage;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (!hitting)
            return;
        foreach (EnemyLife enemy in enemies)
        {
            if (!hitEnemies.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                hitEnemies.Add(enemy);
                print($"Hit {enemy.name}");
                animationController.OnHitConnectEvent(hitIdx);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            var en = other.GetComponent<EnemyLife>();
            if (en != null)
                enemies.Add(en);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            var en = other.GetComponent<EnemyLife>();
            if (en != null)
                enemies.Remove(en);
        }
    }

    public void EnableHitbox(int damage)
    {
        hitting = true;
        this.damage = damage;
        foreach (EnemyLife enemy in enemies)
        {
            enemy.TakeDamage(damage);
            hitEnemies.Add(enemy);
            print($"Hit {enemy.name}");
            animationController.OnHitConnectEvent(hitIdx);
        }
    }

    public void DisableHitbox()
    {
        hitting = false;
        hitEnemies.Clear();
    }
}
