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
    [HideInInspector]
    public Enemies.Cluster currentEncounter;
    [SerializeField] float invicibilityTime = 1f;

    private void Start()
    {
        health = startHealth;
    }

    private void Update()
    {
    }

    public bool TakeDamage(int value)
    {
        if (invincible) return false;

        health -= value;
        if (health <= 0)
        {
            Die();
            print($"Death {health} {startHealth}");
            return true;
        }

        GetComponent<AnimationController>()?.onDamageAnimStart.Invoke(value);

        StartCoroutine(Invicibility(invicibilityTime));

        return true;
    }

    public bool TakeDamage(int value, out bool died)
    {
        died = false;
        if (invincible) return false;

        health -= value;
        if (health <= 0)
        {
            Die();
            died = true;
            return true;
        }

        GetComponent<AnimationController>()?.onDamageAnimStart.Invoke(value);

        StartCoroutine(Invicibility(invicibilityTime));

        return true;
    }

    void Die()
    {
        print("Player is dead!");
        transform.position = spawn.position;
        health = startHealth;
        if (currentEncounter)
            currentEncounter.StopEncounter();
    }

    IEnumerator Invicibility(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }
}

}