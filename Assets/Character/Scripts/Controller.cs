using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Knossos.Character
{

[RequireComponent(typeof(CharacterController), typeof(AnimationController))]
public class Controller : MonoBehaviour
{
    public Config config;

    private CharacterController characterController;
    private AnimationController animationController;
    private VisibilitySystem visibilitySystem;
    private Camera mainCam;

    // Player stats
    private Vector3 movement;
    private Quaternion targetRotation;
    private float _speed = 10f;
    private float targetSpeed;
    public float speed
    {
        get => _speed;
        set => targetSpeed = value;
    }
    private float attMoveSpeedMult = 1f;
    private float attRotSpeedMult = 1f;

    private float _stamina;
    public float stamina
    {
        get => _stamina;
        set => _stamina = Mathf.Clamp(value, 0, config.maxStamina);
    }

    // States
    private bool dashing = false;
    public bool attacking { get => !canDash && !dashing; }

    // Dash control
    private Stopwatch dashTimer = new();
    private bool canDash = true;

    // Run Logic
    private Stopwatch runTimer = new();
    Vector3 previousDirection;
    Vector3 newDirection;

    // Movement Logic

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
        visibilitySystem = GetComponent<VisibilitySystem>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        _stamina = config.maxStamina;
        speed = config.speed;
        targetRotation = Quaternion.LookRotation(transform.forward);
        InputManager.onClearedInput += OnBufferClear;
        runTimer.Start();
        previousDirection = new Vector3(0f, 0f, 1f);
    }

    private void Update()
    {
        _speed = Mathf.Lerp(speed, targetSpeed, .9f);

        if (!dashing) {
            stamina += config.staminaRegenPerSec * Time.deltaTime;
            movement = new Vector3(InputManager.inputs.horizontal, 0, InputManager.inputs.vertical);
            previousDirection = newDirection;
            newDirection = movement.normalized;
            movement = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0) * movement;
            movement = Vector3.ClampMagnitude(movement, 1);
            characterController.Move(movement * Time.deltaTime * speed * attMoveSpeedMult);
        }
        else
            characterController.Move(movement.normalized * Time.deltaTime * speed);

        if (movement.magnitude > 0)
        {
            targetRotation = Quaternion.LookRotation(movement, Vector3.up);

            if (runTimer.IsRunning && runTimer.ElapsedMilliseconds > config.runTimer * 1000f)
                StartRunning();

            if (Vector3.Dot(previousDirection, newDirection) < .5f)
            {
                RestartRunTimer();
            }
        }
        else
            RestartRunTimer();
        if (visibilitySystem.isDetected)
            RestartRunTimer();
        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, config.rotationSpeed * attRotSpeedMult * Time.deltaTime);

        UpdateDash();
        UpdateAttack();
        UpdateSpecial();

    }

    public void RestartRunTimer()
    {
        runTimer.Restart();
        StopRunning();
    }

    public void StartRunning()
    {
        // animationController.SetMoveSpeed(config.runSpeed / config.speed);
        // speed = config.runSpeed;
    }

    public void StopRunning()
    {
        // animationController.SetMoveSpeed(1f);
        // speed = config.speed;
    }

    private void OnBufferClear(BufferedInput input)
    {
        if (input == BufferedInput.Dodge)
            animationController.ResetDodge();
        else if (input == BufferedInput.Attack)
            animationController.ResetAttack();
    }

    // Dash Control

    private void UpdateDash()
    {
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
        if (movement.magnitude == 0f)
        {
            movement = transform.forward;
        }

        stamina -= config.dashCost;
        dashTimer.Restart();

        while (dashTimer.ElapsedMilliseconds < config.dashDuration * 1000f) {
            float curveValue = config.DashSpeedCurve.Evaluate((float)dashTimer.ElapsedMilliseconds / (config.dashDuration * 1000f));
            speed = (curveValue * config.dashStrength + 1) * config.speed;
            yield return null;
        }

        speed = config.speed;
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

    // Attack Control

    private void UpdateAttack()
    {
        if (InputManager.CheckBuffer(BufferedInput.Attack, false))
        {
            canDash = false;
            EnableSlowMotion();
            animationController.ResetDodge();
            animationController.ResetSpecial();
            animationController.TriggerAttack();
        }
    }

    private void UpdateSpecial()
    {
        if (InputManager.CheckBuffer(BufferedInput.Special, false))
        {
            canDash = false;
            EnableSlowMotion();
            animationController.ResetDodge();
            animationController.ResetAttack();
            animationController.TriggerSpecial();
        }
    }

    public void EnableSlowMotion()
    {
        attMoveSpeedMult = config.attackMoveSpeedMultiplier;
        attRotSpeedMult = config.attackRotationSpeedMultiplier;
    }

    public void DisableSlowMotion()
    {
        attMoveSpeedMult = 1f;
        attRotSpeedMult = 1f;
    }

    public void StopAttacking()
    {
        canDash = true;
        DisableSlowMotion();
    }
}

}