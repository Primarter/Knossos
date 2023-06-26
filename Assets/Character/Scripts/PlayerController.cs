using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]//, typeof(AnimationController))]
public class PlayerController : MonoBehaviour
{
    public PlayerConfig config;

    private CharacterController characterController;
    private Knossos.Player.AnimationController animationController;
    private Camera mainCam;

    // Player stats
    private Vector3 movement;
    private Quaternion targetRotation;
    [System.NonSerialized]
    public float speed = 10f;
    private float attMoveSpeedMult = 1f;
    private float attRotSpeedMult = 1f;

    private float _stamina;
    public float stamina
    {
        get => _stamina;
        set => _stamina = Mathf.Clamp(value, 0, config.maxStamina);
    }

    // States
    private bool attacking = false;
    private bool dashing = false;

    // Dash control
    private Stopwatch dashTimer = new Stopwatch();
    [System.NonSerialized]
    private bool canDash = true;

    // Movement Logic

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<Knossos.Player.AnimationController>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        _stamina = config.maxStamina;
        speed = config.speed;
        targetRotation = Quaternion.LookRotation(transform.forward);
        InputManager.onClearedInput += OnBufferClear;
    }

    private void Update()
    {
        if (!dashing) {
            stamina += config.staminaRegenPerSec * Time.deltaTime;
            movement = new Vector3(InputManager.inputs.horizontal, 0, InputManager.inputs.vertical);
            movement = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0) * movement;
            movement = Vector3.ClampMagnitude(movement, 1);
            characterController.Move(movement * Time.deltaTime * speed * attMoveSpeedMult);
        }
        else
            characterController.Move(movement.normalized * Time.deltaTime * speed);

        if (movement.magnitude > 0)
        {
            targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        }
        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, config.rotationSpeed * attRotSpeedMult * Time.deltaTime);

        UpdateDash();
        UpdateAttack();
    }

    private void OnBufferClear(BufferedInput input)
    {
        // if (input == BufferedInput.Dodge)
            // animationController.ResetDodge();
        // else if (input == BufferedInput.Attack)
            // animationController.ResetAttack();
        print("OnBufferClear");
    }

    // Dash Control

    private void UpdateDash()
    {
        if (canDash && stamina >= config.dashCost && InputManager.CheckBuffer(BufferedInput.Dodge, false))
        {
            animationController.TransitionTo(Knossos.Player.State.Dodge, 0.035f);
            // animationController.TriggerDodge();
            // animationController.ResetAttack();
        }
    }

    private IEnumerator DashCoroutine()
    {
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
        if (!dashing && InputManager.CheckBuffer(BufferedInput.Attack, false))
        {
            attacking = true;
            canDash = false;
            EnableSlowMotion();
            // animationController.ResetDodge();
            // animationController.TriggerAttack();
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
        attacking = false;
        canDash = true;
        DisableSlowMotion();
    }
}
