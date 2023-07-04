using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Hitbox : MonoBehaviour
{
    [SerializeField]
    AnimationController animationController;
    public int hitIdx = 0;
    [SerializeField]
    GameObject damageParticle;

    Transform player;
    BoxCollider collider;
    List<EnemyLife> enemies = new();
    List<EnemyLife> hitEnemies = new();
    bool hitting = false;
    int damage;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        enemies.RemoveAll(en => en == null || !en.isActiveAndEnabled);
        if (!hitting)
            return;
        foreach (EnemyLife enemy in enemies)
        {
            if (!hitEnemies.Contains(enemy))
            {
                Hit(enemy);
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

    private void Hit(EnemyLife enemy)
    {
        enemy.TakeDamage(damage);
        hitEnemies.Add(enemy);
        print($"Hit {enemy.name}");
        animationController.OnHitConnectEvent(hitIdx);
        Destroy(GameObject.Instantiate(damageParticle, enemy.transform.position, player.rotation), .5f);
    }

    public void EnableHitbox(int damage)
    {
        hitting = true;
        this.damage = damage;
        foreach (EnemyLife enemy in enemies)
        {
            Hit(enemy);
        }
    }

    public void DisableHitbox()
    {
        hitting = false;
        hitEnemies.Clear();
    }
}
