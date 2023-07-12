using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knossos.Enemies
{


public class OnHitEventSystem : MonoBehaviour
{
    public struct HitInfo
    {
        public int damage;
        public float staggerDuration;
        public float knockBackStrength;
        public float knockBackDuration;
        public Vector3 hitDirection;
    }
    public Material damageMaterial;

    public delegate void OnHit(HitInfo hitInfo);
    public OnHit onHitCallbacks;

    public UnityEvent<HitInfo> onHitEvent = new();

    private Renderer rend;
    private Material regularMaterial;

    private void Awake() {
        rend = GetComponent<Renderer>();
        regularMaterial = rend.sharedMaterial;
    }

    private void Start() {
    }

    public void TakeDamage(Character.MoveInfo moveInfo = new(), Vector3 hitDirection = new())
    {
        HitInfo hitInfo = new HitInfo {
            damage=moveInfo.damage,
            staggerDuration=moveInfo.staggerDuration,
            knockBackStrength=moveInfo.knockBackStrength,
            knockBackDuration=moveInfo.knockBackDuration,
            hitDirection=hitDirection,
        };

        StartCoroutine(MaterialChangeCoroutine());
        if (onHitCallbacks != null)
            onHitCallbacks(hitInfo);
        onHitEvent.Invoke(hitInfo);
    }

    IEnumerator MaterialChangeCoroutine()
    {
        int frame = Time.frameCount;
        rend.material = damageMaterial;

        while (Time.frameCount < frame + 10)
        {
            yield return null;
        }
        rend.material = regularMaterial;
    }
}

}