using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

public class Health : MonoBehaviour
{
    [SerializeField] Transform spawn;
    [SerializeField] int _startHealth = 100;
    public int startHealth
    {
        get => _startHealth;
        private set => _startHealth = value;
    }

    int _health;
    public int health
    {
        get => _health;
        private set => _health = Mathf.Clamp(value, 0, startHealth);
    }

    [HideInInspector]
    public bool invincible = false;
    [SerializeField] float invicibilityTime = 1f;

    private void Start()
    {
        health = startHealth;
    }

    public bool TakeDamage(int value)
    {
        if (invincible) return false;

        health -= value;
        if (health <= 0)
        {
            Die();
            return false;
        }

        GetComponent<AnimationController>()?.onDamageAnimStart.Invoke(value);

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
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }
}

}