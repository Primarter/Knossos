using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Knossos.Character
{

[RequireComponent(typeof(VisualEffect), typeof(SphereCollider), typeof(Hitbox))]
public class AOEController : MonoBehaviour
{
    [SerializeField] AnimationCurve hitboxEvolution;

    float lifetime = 0f;

    SphereCollider areaOfEffect;

    float baseRadius;

    private void Awake()
    {
        lifetime = GetComponent<VisualEffect>()?.GetFloat("Lifetime") ?? 0f;
        areaOfEffect = GetComponent<SphereCollider>();
    }

    public void ActivateAOE()
    {
        baseRadius = areaOfEffect.radius;
        StartCoroutine(UpdateAOE());
    }

    IEnumerator UpdateAOE()
    {
        var start = Time.time;

        while (Time.time < start + lifetime)
        {
            float progress = hitboxEvolution.Evaluate((Time.time - start) / lifetime);
            areaOfEffect.radius = baseRadius * progress;
            yield return null;
        }
        areaOfEffect.radius = baseRadius;
    }
}

}