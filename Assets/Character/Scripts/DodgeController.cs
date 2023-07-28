using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

public class DodgeController : MonoBehaviour
{
    // Dash Control

    Controller mainController;
    AnimationController animationController;
    AttackController attackController;
    Config config;

    [HideInInspector] public bool dashing = false;
    [HideInInspector] public bool canDash = true;

    private float _stamina;
    public float stamina
    {
        get => _stamina;
        set => _stamina = Mathf.Clamp(value, 0, config.maxStamina);
    }

    private System.Diagnostics.Stopwatch dashTimer = new();

    private void Awake()
    {
        mainController = GetComponent<Controller>();
        animationController = GetComponent<AnimationController>();
        attackController = GetComponent<AttackController>();
        if (mainController == null || animationController == null || attackController == null) {
            this.enabled = false;
            return;
        }
        config = mainController.config;
    }

    private void Start()
    {
        _stamina = config.maxStamina;
    }

    public void UpdateDodge()
    {
        if (!dashing)
            stamina += config.staminaRegenPerSec * Time.deltaTime;
        if (canDash && stamina >= config.dashCost && InputManager.CheckBuffer(BufferedInput.Dodge, false))
        {
            animationController.TriggerDodge();
            animationController.ResetAttack();
            animationController.ResetSpecial();
        }
    }

    private IEnumerator DashCoroutine()
    {
        Health health = GetComponent<Health>();
        if (health != null)
            health.invincible = true;

        dashing = true;
        canDash = false;
        if (mainController.movement.magnitude == 0f)
        {
            mainController.movement = transform.forward;
        }

        stamina -= config.dashCost;
        dashTimer.Restart();

        while (dashTimer.ElapsedMilliseconds < config.dashDuration * 1000f)
        {
            float curveValue = config.DashSpeedCurve.Evaluate((float)dashTimer.ElapsedMilliseconds / (config.dashDuration * 1000f));
            mainController.speed = (curveValue * config.dashStrength + 1) * config.speed;
            yield return null;
        }

        mainController.speed = config.speed;
        dashing = false;
        if (health != null)
            health.invincible = false;

        yield return new WaitForSeconds(config.dashCooldown);
        canDash = true;
    }

    public void TriggerDash()
    {
        StartCoroutine(DashCoroutine());
    }
}

}