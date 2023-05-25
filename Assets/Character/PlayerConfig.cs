using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerConfig : ScriptableObject
{
    [Header("Player stats")]
    public float speed = 10f;
    public float maxStamina = 2f;

    [Header("Dash config")]
    public float dashStrength = 20f;
    public float dashDuration = .5f;
    public float dashCooldown = .2f;
    public float dashCost = 1f;
    public float staminaRegenPerSec = 1f;
}
