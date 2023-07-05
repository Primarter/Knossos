using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Enemies
{

public class OnHitEventSystem : MonoBehaviour
{
    public Material damageMaterial;

    public delegate void OnHit(int damage);
    public OnHit onHitCallbacks;

    private EnemyStats stats;
    private Renderer renderer;
    private Material regularMaterial;

    private void Awake() {
        renderer = GetComponent<Renderer>();
        regularMaterial = renderer.sharedMaterial;
        stats = GetComponent<Enemy>().stats;
    }

    private void Start() {
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(MaterialChangeCoroutine());
        if (onHitCallbacks != null) {
            onHitCallbacks(damage);
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

}