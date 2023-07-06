using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] Transform spawn;

    int _health = 100;
    int health
    {
        get => _health;
        set => _health = Mathf.Clamp(value, 0, 100);
    }

    bool invicible = false;
    float invicibilityTime = 1f;

    public bool TakeDamage(int value)
    {
        if (invicible) return false;

        health -= value;
        if (health <= 0)
        {
            Die();
            return false;
        }

        StartCoroutine(Invicibility(invicibilityTime));

        return true;
    }

    void Die()
    {
        print("Player is dead!");
        transform.position = spawn.position;
        health = 100;
    }

    IEnumerator Invicibility(float time)
    {
        invicible = true;
        yield return new WaitForSeconds(time);
        invicible = false;
    }
}
