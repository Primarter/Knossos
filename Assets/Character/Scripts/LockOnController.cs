using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class LockOnController : MonoBehaviour
{
    List<Enemy> enemies = new();

    private Enemy _closest;
    public Enemy lockedEnemy
    {
        get => _closest;
        private set => _closest = value;
    }

    private void Update()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.stats.detectionPriority < 0)
                continue;
            if (lockedEnemy == null || enemy.stats.detectionPriority > lockedEnemy.stats.detectionPriority)
            {
                lockedEnemy = enemy;
            }
            else if (enemy.stats.detectionPriority == lockedEnemy.stats.detectionPriority)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position)
                < Vector3.Distance(lockedEnemy.transform.position, transform.position))
                    lockedEnemy = enemy;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy == null)
            {
                Debug.LogError("LockOnEnemy: " + other.name + " doesn't have Enemy component, discarded");
                return;
            }
            enemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy == null)
                return;

            enemies.Remove(enemy);
            if (lockedEnemy == enemy)
                lockedEnemy = null;
        }
    }

    // In player update:
    // if (movement.magnitude > 0)
    // {
    //     if (movement.magnitude > .85 || lockOnController.lockedEnemy == null)
    //         targetRotation = Quaternion.LookRotation(movement, Vector3.up);
    //     else
    //     {
    //         targetRotation.SetLookRotation(lockOnController.lockedEnemy.transform.position - transform.position);
    //     }
    // }
    // else if (lockOnController.lockedEnemy != null)
    // {
    //     targetRotation.SetLookRotation(lockOnController.lockedEnemy.transform.position - transform.position, Vector3.up);
    // }
}
