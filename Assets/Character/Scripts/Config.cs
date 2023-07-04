using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

[CreateAssetMenu()]
public class Config : ScriptableObject
{
    [Header("Player stats")]
    public float speed = 10f;
    public float attackMoveSpeedMultiplier = .01f;
    public float attackRotationSpeedMultiplier = .2f;
    public float maxStamina = 2f;

    [Header("ControllerConfig")]
    public float rotationSpeed = 10f;

    [Header("Dash config")]
    public AnimationCurve DashSpeedCurve = new();
    public float dashStrength = 20f;
    [Tooltip("Dash animation duration is 1.05s")]
    public float dashDuration = .5f;
    public float dashCooldown = .2f;
    public float dashCost = 1f;
    public float staminaRegenPerSec = 1f;

    [Header("AttackStats")]
    public int[] hitStops = {2, 2, 4};

    [Header("Read Only Info")]
    public float DodgeAnimationSpeedMultiplier = 1f;

    private void OnValidate()
    {
        DodgeAnimationSpeedMultiplier = 1.05f / dashDuration;
    }
}

}