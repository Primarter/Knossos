using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class HitboxManager : MonoBehaviour
{
    [SerializeField]
    Hitbox[] comboHitboxes;

    public void TriggerHitbox(int hitboxIdx)
    {
        comboHitboxes[hitboxIdx].EnableHitbox(1);
    }

    public void DisableHitbox(int hitboxIdx)
    {
        comboHitboxes[hitboxIdx].DisableHitbox();
    }
}
