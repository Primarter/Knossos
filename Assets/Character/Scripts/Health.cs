using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
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
            print("Player is dead!");
            return false;
        }

        StartCoroutine(Invicibility(invicibilityTime));
        return true;
    }

    IEnumerator Invicibility(float time)
    {
        invicible = true;
        yield return new WaitForSeconds(time);
        invicible = false;
    }
}
