using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyLife : MonoBehaviour
{
    public Material damageMaterial;

    private EnemyStats stats;
    private Renderer renderer;
    private Material regularMaterial;

    float health;

    private void Awake() {
        renderer = GetComponent<Renderer>();
        regularMaterial = renderer.sharedMaterial;
        stats = GetComponent<Enemy>().stats;
    }

    private void Start() {
        health = stats.life;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(MaterialChangeCoroutine());
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator MaterialChangeCoroutine()
    {
        int frame = Time.frameCount;
        renderer.material = damageMaterial;

        while (Time.frameCount < frame + 10)
        {
            yield return null;
        }
        renderer.material = regularMaterial;
    }
}
