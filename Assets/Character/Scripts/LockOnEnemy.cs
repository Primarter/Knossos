using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class LockOnEnemy : MonoBehaviour
{
    List<Transform> enemies = new List<Transform>();

    private Transform _closest;
    public Transform closestEnemy
    {
        get => _closest;
        private set => _closest = value;
    }

    private void Update()
    {
        foreach (Transform enemy in enemies)
        {
            if (Vector3.Distance(enemy.position, transform.position)
                < Vector3.Distance(closestEnemy.position, transform.position))
            {
                closestEnemy = enemy;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            enemies.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Remove(other.transform);
            if (closestEnemy == other.transform)
                closestEnemy = null;
        }
    }
}
