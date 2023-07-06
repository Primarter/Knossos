using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

[RequireComponent(typeof(BoxCollider))]
public class Hitbox : MonoBehaviour
{
    [SerializeField]
    AnimationController animationController;
    public int hitIdx = 0;
    [SerializeField]
    GameObject damageParticle;

    Transform player;
    List<Enemies.OnHitEventSystem> enemies = new();
    List<Enemies.OnHitEventSystem> hitEnemies = new();
    bool hitting = false;
    int damage;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        enemies.RemoveAll(en => en == null || !en.isActiveAndEnabled);
        if (!hitting)
            return;
        foreach (Enemies.OnHitEventSystem enemy in enemies)
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
            var en = other.GetComponent<Enemies.OnHitEventSystem>();
            if (en != null)
                enemies.Add(en);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            var en = other.GetComponent<Enemies.OnHitEventSystem>();
            if (en != null)
                enemies.Remove(en);
        }
    }

    private void Hit(Enemies.OnHitEventSystem enemy)
    {
        enemy.TakeDamage(damage);
        hitEnemies.Add(enemy);
        animationController.OnHitConnectEvent(hitIdx);
        Destroy(GameObject.Instantiate(damageParticle, enemy.transform.position, player.rotation), .5f);
    }

    public void EnableHitbox(int damage)
    {
        hitting = true;
        this.damage = damage;
        foreach (Enemies.OnHitEventSystem enemy in enemies)
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

}