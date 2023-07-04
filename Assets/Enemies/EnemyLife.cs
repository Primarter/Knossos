using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    public float health = 3;
    public Material damageMaterial;

    private Renderer renderer;
    private Material regularMaterial;

    private void Awake() {
        renderer = GetComponent<Renderer>();
        regularMaterial = renderer.sharedMaterial;
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
