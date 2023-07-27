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

    VisualEffect specialBurst;

    SphereCollider areaOfEffect;

    Hitbox hitbox;

    float baseRadius;

    private void Awake()
    {
        specialBurst = GetComponent<VisualEffect>();
        areaOfEffect = GetComponent<SphereCollider>();
        hitbox = GetComponent<Hitbox>();
    }

    public void ActivateAOE()
    {
        specialBurst.Play();
        baseRadius = areaOfEffect.radius;
        hitbox.EnableHitbox();
        StartCoroutine(UpdateAOE(specialBurst.GetFloat("Lifetime")));
    }

    IEnumerator UpdateAOE(float lifetime)
    {
        var start = Time.time;

        while (Time.time < start + lifetime)
        {
            float progress = hitboxEvolution.Evaluate((Time.time - start) / lifetime);
            areaOfEffect.radius = baseRadius * progress;
            yield return null;
        }
        areaOfEffect.radius = baseRadius;
        hitbox.DisableHitbox();
    }
}

}